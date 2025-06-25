from pathlib import Path

FILE_PATH = Path(__file__).parent

DEST_PATH = FILE_PATH.parent / "RouletteRecorder" / "Constant"
GLOBAL_DEST_PATH = DEST_PATH / "OpcodeGlobal.cs"

GLOBAL_OPCODE_URL = "https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/refs/heads/master/FFXIVOpcodes/Ipcs.cs"
CN_OPCODE_DOWNLOAD_URL = (
    "https://github.com/moewcorp/FFXIVNetworkOpcodes/archive/refs/heads/master.zip"
)
