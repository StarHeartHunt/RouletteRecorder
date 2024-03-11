import csv
import json

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

    with open("data/ContentFinderCondition.csv", encoding="utf-8") as f:
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
                        "level": row["ClassJobLevelRequired"],
                        "levelSync": row["ClassJobLevelSync"],
                        "item": row["ItemLevelRequired"],
                        "itemSync": row["ItemLevelSync"],
                        "memberType": row["ContentMemberType"],
                    }
                else:
                    print(f"Skipping {id_}")

                continue

            data[id_]["name"]["chs"] = row["Name"]

    with open("output/instance.json", "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
