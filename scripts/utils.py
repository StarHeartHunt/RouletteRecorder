from io import TextIOWrapper


def extract_header(f: TextIOWrapper):
    headers = []
    line_num = 0

    for line in f:
        splited = line.strip().split(",")
        if line_num == 0:
            headers = splited
        elif line_num == 1:
            for index, piece in enumerate(splited):
                if piece == "#":
                    headers[index] = "Key"
                else:
                    headers[index] = piece
        elif line_num == 2:
            break

        line_num += 1

    return headers
