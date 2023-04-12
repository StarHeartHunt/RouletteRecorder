namespace RouletteRecorder.Packer
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;

    public class Program
    {

        private static void Bundle(string env)
        {
            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, env);
            var entry = Path.Combine(root, "RouletteRecorder.dll");
            var version = FileVersionInfo.GetVersionInfo(entry);

            var outName = $"RouletteRecorder-{version.FileVersion}-{env}.zip";

            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var dll in Directory.GetFiles(root))
                    {
                        archive.CreateEntryFromFile(dll, @"RouletteRecorder/" + Path.GetFileName(dll));
                    }

                    foreach (var data in Directory.GetFiles(Path.Combine(root, "data"), "*.json"))
                    {
                        archive.CreateEntryFromFile(data, @"RouletteRecorder/data/" + Path.GetFileName(data));
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outName), FileMode.Create))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fileStream);
                }
            }
        }

        private static void Main(string[] args)
        {
            string env = args.Length >= 1 ? args[0] : "Release";
            Bundle(env);
        }

    }
}
