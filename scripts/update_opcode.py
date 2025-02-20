from pathlib import Path

import httpx

FILE_PATH = Path(__file__).parent

OPCODES_PATH = FILE_PATH / "opcodes" / "output"

DEST_PATH = FILE_PATH.parent / "RouletteRecorder" / "Constant"
GLOBAL_DEST_PATH = DEST_PATH / "OpcodeGlobal.cs"
CN_DEST_PATH = DEST_PATH / "OpcodeChina.cs"

GLOBAL_OPCODE_URL = "https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/refs/heads/master/FFXIVOpcodes/Ipcs.cs"
CN_OPCODE_URL = "https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/refs/heads/master/FFXIVOpcodes/Ipcs_cn.cs"


def main():
    resp = httpx.get(GLOBAL_OPCODE_URL).raise_for_status()
    print("[INFO]", "Global opcode downloaded")
    status = GLOBAL_DEST_PATH.write_text(resp.text, encoding="utf-8")
    print("[INFO]", "Global opcode written to file with ret:", status)


if __name__ == "__main__":
    main()
