# Contract: FuzzyMatchService

## Doel
Bepalen of twee tekstparen als duplicaat beschouwd moeten worden op basis van fuzzy string matching.

## Interface
- Methode: `bool IsDuplicate(string tekst1, string tekst2, double threshold = 90.0)`
- Input: twee strings, optionele drempelwaarde
- Output: bool (true als duplicaat)

## Testgevallen
- Twee (bijna) gelijke strings → true
- Twee totaal verschillende strings → false
- Drempelwaarde aanpassen beïnvloedt resultaat
