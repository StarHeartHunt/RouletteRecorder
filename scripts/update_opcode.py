import shutil
from pathlib import Path

FILE_PATH = Path(__file__).parent

OPCODE_PATH = FILE_PATH / "opcode" / "output"

DEST_PATH = FILE_PATH.parent / "RouletteRecorder" / "Constant"
GLOBAL_DEST_PATH = DEST_PATH / "OpcodeGlobal.cs"
CN_DEST_PATH = DEST_PATH / "OpcodeChina.cs"


def update_opcode(pattern, src_name, dest):
    opcodes = list(OPCODE_PATH.glob(pattern))
    opcodes.sort(reverse=True)

    latest_opcode = opcodes[0]
    print("[INFO]", "All opcodes:", opcodes)
    print("[INFO]", "Latest opcode:", latest_opcode)

    copied = shutil.copy(latest_opcode.joinpath(src_name), dest)
    print("[INFO]", "Copied:", copied)


def main():
    update_opcode("Global_*", "Ipcs.cs", DEST_PATH.joinpath("OpcodeGlobal.cs"))
    update_opcode("CN_*", "Ipcs_cn.cs", DEST_PATH.joinpath("OpcodeChina.cs"))


if __name__ == "__main__":
    main()
