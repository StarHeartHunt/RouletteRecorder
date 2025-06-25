from consts import GLOBAL_DEST_PATH, GLOBAL_OPCODE_URL
import httpx


def main():
    resp = httpx.get(GLOBAL_OPCODE_URL).raise_for_status()
    print("[INFO]", "Global opcode downloaded")
    status = GLOBAL_DEST_PATH.write_text(resp.text, encoding="utf-8")
    print("[INFO]", "Global opcode written to file with ret:", status)


if __name__ == "__main__":
    main()
