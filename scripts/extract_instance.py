import csv
import json
from pathlib import Path

import httpx
from utils import extract_header

if __name__ == "__main__":
    data = {}

    resp = httpx.get(
        "https://xivapi.com/ContentFinderCondition",
        params={
            "columns": ",".join(
                [
                    "ID",
                    "Name",
                    "Name_ja",
                    "Name_de",
                    "Name_fr",
                    "ClassJobLevelSync",
                    "ClassJobLevelRequired",
                    "ContentType.ID",
                    "ItemLevelRequired",
                    "ItemLevelSync",
                    "ContentMemberType.ID",
                ]
            ),
            "limit": "3000",  # need to implement pagination, 980 total (2024/3/6)
        },
    ).json()
    for result in resp["Results"]:
        if not result["Name"]:
            continue

        data[result["ID"]] = {
            "name": {
                "chs": "",
                "en": result["Name"],
                "ja": result["Name_ja"],
                "de": result["Name_de"],
                "fr": result["Name_fr"],
            },
            "type": result["ContentType"]["ID"],
            "level": result["ClassJobLevelRequired"],
            "levelSync": result["ClassJobLevelSync"],
            "item": result["ItemLevelRequired"],
            "itemSync": result["ItemLevelSync"],
            "memberType": result["ContentMemberType"]["ID"],
        }

    dest = Path(__file__).parent / "data" / "ContentFinderCondition.csv"
    dest.write_bytes(
        httpx.get(
            "https://raw.githubusercontent.com/thewakingsands/ffxiv-datamining-cn/master/ContentFinderCondition.csv"
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
                        "name": {
                            "chs": row["Name"],
                            "en": "",
                            "ja": "",
                            "de": "",
                            "fr": "",
                        },
                        "type": row["ContentType"],
                        "level": row["ClassJobLevel{Required}"],
                        "levelSync": row["ClassJobLevel{Sync}"],
                        "item": row["ItemLevel{Required}"],
                        "itemSync": row["ItemLevel{Sync}"],
                        "memberType": row["ContentMemberType"],
                    }
                else:
                    print(f"Skipping {id_}")

                continue

            data[id_]["name"]["chs"] = row["Name"]

    dest = Path(__file__).parent.parent.joinpath(
        "RouletteRecorder", "data", "instance.json"
    )
    dest.write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding="utf-8")
