
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RouletteRecorderPayload
{

    public sealed class SigScanner : IDisposable
    {
        private IntPtr moduleCopyPtr;

        private long moduleCopyOffset;

        public bool IsCopy { get; }

        public bool Is32BitProcess { get; }

        public IntPtr SearchBase => IsCopy ? moduleCopyPtr : Module.BaseAddress;

        public IntPtr TextSectionBase => new IntPtr(SearchBase.ToInt64() + TextSectionOffset);

        public long TextSectionOffset { get; private set; }

        public int TextSectionSize { get; private set; }

        public IntPtr DataSectionBase => new IntPtr(SearchBase.ToInt64() + DataSectionOffset);

        public long DataSectionOffset { get; private set; }

        public int DataSectionSize { get; private set; }

        public IntPtr RDataSectionBase => new IntPtr(SearchBase.ToInt64() + RDataSectionOffset);

        public long RDataSectionOffset { get; private set; }

        public int RDataSectionSize { get; private set; }

        public ProcessModule Module { get; }

        private IntPtr TextSectionTop => TextSectionBase + TextSectionSize;

        public SigScanner(bool doCopy = false)
            : this(Process.GetCurrentProcess().MainModule, doCopy)
        {
        }

        public SigScanner(ProcessModule module, bool doCopy = false)
        {
            Module = module;
            Is32BitProcess = !Environment.Is64BitProcess;
            IsCopy = doCopy;
            SetupSearchSpace(module);
            if (IsCopy)
            {
                SetupCopiedSegments();
            }
        }

        public static IntPtr Scan(IntPtr baseAddress, int size, string signature)
        {
            (byte[] Needle, bool[] Mask) tuple = ParseSignature(signature);
            byte[] needle = tuple.Needle;
            bool[] mask = tuple.Mask;
            int index = IndexOf(baseAddress, size, needle, mask);
            if (index < 0)
            {
                throw new KeyNotFoundException("Can't find a signature of " + signature);
            }
            return baseAddress + index;
        }

        public static bool TryScan(IntPtr baseAddress, int size, string signature, out IntPtr result)
        {
            try
            {
                result = Scan(baseAddress, size, signature);
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = IntPtr.Zero;
                return false;
            }
        }

        public IntPtr GetStaticAddressFromSig(string signature, int offset = 0)
        {
            IntPtr instrAddr = ScanText(signature);
            instrAddr = IntPtr.Add(instrAddr, offset);
            long bAddr = (long)Module.BaseAddress;
            long num;
            do
            {
                instrAddr = IntPtr.Add(instrAddr, 1);
                num = Marshal.ReadInt32(instrAddr) + (long)instrAddr + 4 - bAddr;
            }
            while ((num < DataSectionOffset || num > DataSectionOffset + DataSectionSize) && (num < RDataSectionOffset || num > RDataSectionOffset + RDataSectionSize));
            return IntPtr.Add(instrAddr, Marshal.ReadInt32(instrAddr) + 4);
        }

        public bool TryGetStaticAddressFromSig(string signature, out IntPtr result, int offset = 0)
        {
            try
            {
                result = GetStaticAddressFromSig(signature, offset);
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = IntPtr.Zero;
                return false;
            }
        }

        public IntPtr ScanData(string signature)
        {
            IntPtr scanRet = Scan(DataSectionBase, DataSectionSize, signature);
            if (IsCopy)
            {
                scanRet = new IntPtr(scanRet.ToInt64() - moduleCopyOffset);
            }
            return scanRet;
        }

        public bool TryScanData(string signature, out IntPtr result)
        {
            try
            {
                result = ScanData(signature);
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = IntPtr.Zero;
                return false;
            }
        }

        public IntPtr ScanModule(string signature)
        {
            IntPtr scanRet = Scan(SearchBase, Module.ModuleMemorySize, signature);
            if (IsCopy)
            {
                scanRet = new IntPtr(scanRet.ToInt64() - moduleCopyOffset);
            }
            return scanRet;
        }

        public bool TryScanModule(string signature, out IntPtr result)
        {
            try
            {
                result = ScanModule(signature);
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = IntPtr.Zero;
                return false;
            }
        }

        public IntPtr ResolveRelativeAddress(IntPtr nextInstAddr, int relOffset)
        {
            if (Is32BitProcess)
            {
                throw new NotSupportedException("32 bit is not supported.");
            }
            return nextInstAddr + relOffset;
        }

        public IntPtr ScanText(string signature)
        {
            IntPtr mBase = (IsCopy ? moduleCopyPtr : TextSectionBase);
            IntPtr scanRet = Scan(mBase, TextSectionSize, signature);
            if (IsCopy)
            {
                scanRet = new IntPtr(scanRet.ToInt64() - moduleCopyOffset);
            }
            byte insnByte = Marshal.ReadByte(scanRet);
            if (insnByte == 232 || insnByte == 233)
            {
                return ReadJmpCallSig(scanRet);
            }
            return scanRet;
        }

        public bool TryScanText(string signature, out IntPtr result)
        {
            try
            {
                result = ScanText(signature);
                return true;
            }
            catch (KeyNotFoundException)
            {
                result = IntPtr.Zero;
                return false;
            }
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(moduleCopyPtr);
        }

        private static IntPtr ReadJmpCallSig(IntPtr sigLocation)
        {
            int jumpOffset = Marshal.ReadInt32(sigLocation, 1);
            return IntPtr.Add(sigLocation, 5 + jumpOffset);
        }

        private static (byte[] Needle, bool[] Mask) ParseSignature(string signature)
        {
            signature = signature.Replace(" ", string.Empty);
            if (signature.Length % 2 != 0)
            {
                throw new ArgumentException("Signature without whitespaces must be divisible by two.", "signature");
            }
            int needleLength = signature.Length / 2;
            byte[] needle = new byte[needleLength];
            bool[] mask = new bool[needleLength];
            for (int i = 0; i < needleLength; i++)
            {
                string hexString = signature.Substring(i * 2, 2);
                if (hexString == "??" || hexString == "**")
                {
                    needle[i] = 0;
                    mask[i] = true;
                }
                else
                {
                    needle[i] = byte.Parse(hexString, NumberStyles.AllowHexSpecifier);
                    mask[i] = false;
                }
            }
            return (needle, mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe static int IndexOf(IntPtr bufferPtr, int bufferLength, byte[] needle, bool[] mask)
        {
            if (needle.Length > bufferLength)
            {
                return -1;
            }
            int[] badShift = BuildBadCharTable(needle, mask);
            int last = needle.Length - 1;
            int offset = 0;
            int maxoffset = bufferLength - needle.Length;
            for (byte* buffer = (byte*)(void*)bufferPtr; offset <= maxoffset; offset += badShift[(buffer + offset)[last]])
            {
                int position = last;
                while (needle[position] == (buffer + position)[offset] || mask[position])
                {
                    if (position == 0)
                    {
                        return offset;
                    }
                    position--;
                }
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int[] BuildBadCharTable(byte[] needle, bool[] mask)
        {
            int last = needle.Length - 1;
            int[] badShift = new int[256];
            int idx = last;
            while (idx > 0 && !mask[idx])
            {
                idx--;
            }
            int diff = last - idx;
            if (diff == 0)
            {
                diff = 1;
            }
            for (idx = 0; idx <= 255; idx++)
            {
                badShift[idx] = diff;
            }
            for (idx = last - diff; idx < last; idx++)
            {
                badShift[needle[idx]] = last - idx;
            }
            return badShift;
        }

        private void SetupSearchSpace(ProcessModule module)
        {
            IntPtr baseAddress = module.BaseAddress;
            int ntNewOffset = Marshal.ReadInt32(baseAddress, 60);
            IntPtr ntHeader = baseAddress + ntNewOffset;
            IntPtr fileHeader = ntHeader + 4;
            short numSections = Marshal.ReadInt16(ntHeader, 6);
            IntPtr optionalHeader = fileHeader + 20;
            IntPtr sectionHeader = ((!Is32BitProcess) ? (optionalHeader + 240) : (optionalHeader + 224));
            IntPtr sectionCursor = sectionHeader;
            for (int i = 0; i < numSections; i++)
            {
                switch (Marshal.ReadInt64(sectionCursor))
                {
                    case 500236121134L:
                        TextSectionOffset = Marshal.ReadInt32(sectionCursor, 12);
                        TextSectionSize = Marshal.ReadInt32(sectionCursor, 8);
                        break;
                    case 418564367406L:
                        DataSectionOffset = Marshal.ReadInt32(sectionCursor, 12);
                        DataSectionSize = Marshal.ReadInt32(sectionCursor, 8);
                        break;
                    case 107152478073390L:
                        RDataSectionOffset = Marshal.ReadInt32(sectionCursor, 12);
                        RDataSectionSize = Marshal.ReadInt32(sectionCursor, 8);
                        break;
                }
                sectionCursor += 40;
            }
        }

        private unsafe void SetupCopiedSegments()
        {
            moduleCopyPtr = Marshal.AllocHGlobal(Module.ModuleMemorySize);
            Buffer.MemoryCopy(Module.BaseAddress.ToPointer(), moduleCopyPtr.ToPointer(), Module.ModuleMemorySize, Module.ModuleMemorySize);
            moduleCopyOffset = moduleCopyPtr.ToInt64() - Module.BaseAddress.ToInt64();
        }

        public unsafe IntPtr ScanReversed(IntPtr baseAddress, int size, string signature)
        {
            byte?[] needle = SigToNeedle(signature);
            byte* pCursor = (byte*)baseAddress.ToPointer();
            byte* pBottom = (byte*)(void*)(baseAddress - size + needle.Length);
            while (pCursor > pBottom)
            {
                if (IsMatch(pCursor, needle))
                {
                    return (IntPtr)pCursor;
                }
                pCursor--;
            }
            throw new KeyNotFoundException("Can't find a signature of " + signature);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe bool IsMatch(byte* pCursor, byte?[] needle)
        {
            for (int i = 0; i < needle.Length; i++)
            {
                byte? expected = needle[i];
                if (expected.HasValue)
                {
                    byte actual = pCursor[i];
                    if (expected != actual)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte?[] SigToNeedle(string signature)
        {
            signature = signature.Replace(" ", "");
            if (signature.Length % 2 != 0)
            {
                throw new ArgumentException("Signature without whitespaces must be divisible by two.", "signature");
            }
            int needleLength = signature.Length / 2;
            byte?[] needle = new byte?[needleLength];
            for (int i = 0; i < needleLength; i++)
            {
                string hexString = signature.Substring(i * 2, 2);
                if (hexString == "??" || hexString == "**")
                {
                    needle[i] = null;
                }
                else
                {
                    needle[i] = byte.Parse(hexString, NumberStyles.AllowHexSpecifier);
                }
            }
            return needle;
        }
    }

}
