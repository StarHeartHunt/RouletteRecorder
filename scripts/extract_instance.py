import csv
import json
from pathlib import Path

import httpx
from utils import extract_header

PAGE_LIMIT = 500
FIELDS = ",".join(
    [
        "Name",
        "Name@lang(ja)",
        "Name@lang(de)",
        "Name@lang(fr)",
        "ClassJobLevelSync",
        "ClassJobLevelRequired",
        "ContentType.value",
        "ItemLevelRequired",
        "ItemLevelSync",
        "ContentMemberType.value",
    ]
)
client = httpx.Client()

if __name__ == "__main__":
    data = {}
    all_parsed_rows = []

    while True:
        params = {
            "fields": FIELDS,
            "limit": PAGE_LIMIT,
        }
        if len(all_parsed_rows) > 0:
            params["after"] = all_parsed_rows[-1]["row_id"]

        resp = client.get(
            "https://beta.xivapi.com/api/1/sheet/ContentFinderCondition",
            params=params,
        )
        resp.raise_for_status()

        parsed = resp.json()
        all_parsed_rows.extend(parsed["rows"])
        if len(parsed["rows"]) == PAGE_LIMIT:
            continue
        else:
            break

    for row in all_parsed_rows:
        fields = row["fields"]
        if not fields["Name"]:
            continue

        data[int(row["row_id"])] = {
            "name": {
                "chs": "",
                "en": fields["Name"],
                "ja": fields["Name@lang(ja)"],
                "de": fields["Name@lang(de)"],
                "fr": fields["Name@lang(fr)"],
            },
            "type": int(fields["ContentType"]["value"]),
            "level": int(fields["ClassJobLevelRequired"]),
            "levelSync": int(fields["ClassJobLevelSync"]),
            "item": int(fields["ItemLevelRequired"]),
            "itemSync": int(fields["ItemLevelSync"]),
            "memberType": int(fields["ContentMemberType"]["value"]),
        }

    dest = Path(__file__).parent / "data" / "ContentFinderCondition.csv"
    dest.write_bytes(
        client.get(
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
                        "type": int(row["ContentType"]),
                        "level": int(row["ClassJobLevel{Required}"]),
                        "levelSync": int(row["ClassJobLevel{Sync}"]),
                        "item": int(row["ItemLevel{Required}"]),
                        "itemSync": int(row["ItemLevel{Sync}"]),
                        "memberType": int(row["ContentMemberType"]),
                    }
                else:
                    print(f"Skipping {id_}")

                continue

            data[id_]["name"]["chs"] = row["Name"]

    dest = Path(__file__).parent.parent.joinpath(
        "RouletteRecorder", "data", "instance.json"
    )
    dest.write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding="utf-8")
