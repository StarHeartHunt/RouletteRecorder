import shutil
from pathlib import Path

import httpx

FILE_PATH = Path(__file__).parent

OPCODES_PATH = FILE_PATH / "opcodes" / "output"

DEST_PATH = FILE_PATH.parent / "RouletteRecorder" / "Constant"
GLOBAL_DEST_PATH = DEST_PATH / "OpcodeGlobal.cs"
CN_DEST_PATH = DEST_PATH / "OpcodeChina.cs"

GLOBAL_OPCODE_URL = "https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/FFXIVOpcodes/Ipcs.cs"


def update_opcode(pattern, src_name, dest):
    opcodes = list(OPCODES_PATH.glob(pattern))
    opcodes.sort(reverse=True)

    latest_opcode = opcodes[0]
    print("[INFO]", "All opcodes:", opcodes)
    print("[INFO]", "Latest opcode:", latest_opcode)

    copied = shutil.copy(latest_opcode.joinpath(src_name), dest)
    print("[INFO]", "Copied:", copied)


def main():
    update_opcode("CN_*", "Ipcs_cn.cs", DEST_PATH.joinpath("OpcodeChina.cs"))

    resp = httpx.get(GLOBAL_OPCODE_URL).raise_for_status()
    print("[INFO]", "Global opcode downloaded")
    status = GLOBAL_DEST_PATH.write_text(resp.text, encoding="utf-8")
    print("[INFO]", "Global opcode written to file with ret:", status)


if __name__ == "__main__":
    main()
