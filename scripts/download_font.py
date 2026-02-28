import urllib.request
import ssl

ctx = ssl.create_default_context()
ctx.check_hostname = False
ctx.verify_mode = ssl.CERT_NONE

# Alternative source raw GitHub link for Roboto-Regular (Hinted)
url = "https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Regular.ttf"
output = "src/IncriElemental.Desktop/Content/Arial.ttf"

# Add User-Agent header to avoid being blocked by some CDNs
headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
}

try:
    req = urllib.request.Request(url, headers=headers)
    with urllib.request.urlopen(req, context=ctx) as response, open(output, 'wb') as out_file:
        out_file.write(response.read())
    print("Font downloaded successfully.")
except Exception as e:
    print(f"Failed to download font: {e}")
