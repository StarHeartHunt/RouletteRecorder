import csv
import json
from pathlib import Path

import httpx
from utils import extract_header

if __name__ == "__main__":
    data = {}
    i18n_names = {}
    resp = httpx.get(
        "https://xivapi.com/ContentRoulette",
        params={
            "columns": ",".join(
                [
                    "ID",
                    "Name",
                    "Name_ja",
                    "Name_de",
                    "Name_fr",
                    "IsInDutyFinder",
                    "IsGoldSaucer",
                ]
            ),
            "limit": "3000",
        },
    ).json()
    for result in resp["Results"]:
        if result["IsInDutyFinder"] == 0 or result["IsGoldSaucer"] == 1:
            continue

        i18n_names[str(result["ID"])] = {
            "en": result["Name"],
            "ja": result["Name_ja"],
            "de": result["Name_de"],
            "fr": result["Name_fr"],
        }

    with open("data/ContentRoulette.csv", encoding="utf-8") as f:
        headers = extract_header(f)
        reader = csv.DictReader(f, fieldnames=headers)
        for row in reader:
            if row["Name"] and i18n_names.get(row["Key"]) is not None:
                name = {
                    "chs": row["Name"],
                }
                name.update(i18n_names[row["Key"]])
                data[row["Key"]] = name

    dest = Path(__file__).parent.parent.joinpath(
        "RouletteRecorder", "data", "roulette.json"
    )
    dest.write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding="utf-8")
