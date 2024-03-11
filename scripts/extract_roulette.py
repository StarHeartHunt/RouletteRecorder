import csv
import json

import httpx

from utils import extract_header

if __name__ == "__main__":
    data = {}
    i18n_names = {}
    resp = httpx.get(
        "https://xivapi.com/ContentRoulette",
        params={"columns": "ID,Name,Name_ja,Name_de,Name_fr", "limit": "3000"},
    ).json()
    for result in resp["Results"]:
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
            if (
                row["Name"]
                and row["IsInDutyFinder"] == "True"
                and row["IsGoldSaucer"] == "False"
            ):
                name = {
                    "chs": row["Name"],
                }
                name.update(i18n_names[row["Key"]])
                data[row["Key"]] = name

    with open("output/roulette.json", "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
