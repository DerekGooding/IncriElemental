import urllib.request
import ssl

ctx = ssl.create_default_context()
ctx.check_hostname = False
ctx.verify_mode = ssl.CERT_NONE

url = "https://fonts.gstatic.com/s/roboto/v30/KFOmCnqEu92Fr1Mu4mxK.ttf"
output = "src/IncriElemental.Desktop/Content/font.ttf"

try:
    with urllib.request.urlopen(url, context=ctx) as response, open(output, 'wb') as out_file:
        out_file.write(response.read())
    print("Font downloaded successfully.")
except Exception as e:
    print(f"Failed to download font: {e}")
