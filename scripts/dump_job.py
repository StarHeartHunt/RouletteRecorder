import csv
import json
from pathlib import Path

import httpx
from utils import extract_header

client = httpx.Client()

if __name__ == "__main__":
    data = {}

    dest = Path(__file__).parent / "data" / "ClassJob.csv"
    dest.write_bytes(
        client.get(
            "https://raw.githubusercontent.com/thewakingsands/ffxiv-datamining-cn/refs/heads/master/ClassJob.csv"
        ).read()
    )

    with open(dest.resolve(), encoding="utf-8") as f:
        headers = extract_header(f)
        reader = csv.DictReader(f, fieldnames=headers)
        for row in reader:
            id_ = int(row["Key"])

            if not data.get(id_):
                if row["Name"]:
                    data[id_] = {
                        "name": row["Name"],
                    }
                else:
                    print(f"Skipping {id_}")

                continue

    dest = Path(__file__).parent.parent.joinpath("RouletteRecorder", "data", "job.json")
    dest.write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding="utf-8")
