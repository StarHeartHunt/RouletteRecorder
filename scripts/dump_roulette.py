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
        "IsInDutyFinder",
        "IsGoldSaucer",
    ]
)
client = httpx.Client()

if __name__ == "__main__":
    data = {}
    i18n_names = {}
    all_parsed_rows = []

    while True:
        params = {
            "fields": FIELDS,
            "limit": PAGE_LIMIT,
        }
        if len(all_parsed_rows) > 0:
            params["after"] = all_parsed_rows[-1]["row_id"]

        resp = client.get(
            "https://beta.xivapi.com/api/1/sheet/ContentRoulette",
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
        if fields["IsInDutyFinder"] is False or fields["IsGoldSaucer"] is True:
            continue

        i18n_names[str(row["row_id"])] = {
            "en": fields["Name"],
            "ja": fields["Name@lang(ja)"],
            "de": fields["Name@lang(de)"],
            "fr": fields["Name@lang(fr)"],
        }

    dest = Path(__file__).parent / "data" / "ContentRoulette.csv"
    dest.write_bytes(
        client.get(
            "https://raw.githubusercontent.com/thewakingsands/ffxiv-datamining-cn/master/ContentRoulette.csv"
        ).read()
    )

    with open(dest.resolve(), encoding="utf-8") as f:
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
