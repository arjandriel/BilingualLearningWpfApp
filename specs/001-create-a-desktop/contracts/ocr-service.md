# Contract: OcrService

## Doel
Tekst extraheren uit een afbeelding (bitmap) en teruggeven als string.

## Interface
- Methode: `string ExtractText(Bitmap image)`
- Input: Bitmap (afbeelding)
- Output: string (herkende tekst, tab-gescheiden per regel)
- Fouten: Gooit exception bij niet-herkenbare afbeelding of ontbrekende tessdata

## Testgevallen
- Geldige afbeelding met tekst → retourneert tekst
- Lege afbeelding → retourneert lege string
- Afbeelding zonder tessdata → exception
