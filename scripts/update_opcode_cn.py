import asyncio
import io
from pathlib import Path
import shutil
import tempfile
import zipfile

from consts import CN_OPCODE_DOWNLOAD_URL, DEST_PATH
import httpx

OPCODE_PATH = tempfile.TemporaryDirectory()


def update_opcode(pattern, src_name, dest):
    opcodes = list(
        Path(OPCODE_PATH.name)
        .joinpath("FFXIVNetworkOpcodes-master", "output")
        .glob(pattern)
    )
    opcodes.sort(reverse=True)

    latest_opcode = opcodes[0]
    print("[INFO]", "All opcodes:", [x.name for x in opcodes])
    print("[INFO]", "Latest opcode:", latest_opcode)

    copied = shutil.copy(latest_opcode.joinpath(src_name), dest)
    print("[INFO]", "Copied:", copied)


async def main():
    async with httpx.AsyncClient() as client:
        resp = await client.get(CN_OPCODE_DOWNLOAD_URL, follow_redirects=True)
        resp.raise_for_status()

        with zipfile.ZipFile(io.BytesIO(resp.content)) as file:
            file.extractall(OPCODE_PATH.name)
        print("[INFO]", "China opcode downloaded")

    print(OPCODE_PATH.name)
    update_opcode("CN_*", "Ipcs_cn.cs", DEST_PATH.joinpath("OpcodeChina.cs"))

    OPCODE_PATH.cleanup()


if __name__ == "__main__":
    asyncio.run(main())
