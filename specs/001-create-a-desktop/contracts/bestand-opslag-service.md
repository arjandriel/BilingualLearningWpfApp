# Contract: BestandOpslagService

## Doel
Tekstparen en metadata opslaan en laden van/naar JSON-bestand.

## Interface
- Methode: `void SaveBestand(Bestand bestand, string pad)`
- Methode: `Bestand LoadBestand(string pad)`
- Input: Bestand-object, bestandsnaam
- Output: Bestand-object (bij laden)
- Fouten: Gooit exception bij I/O-fouten of ongeldig JSON

## Testgevallen
- Geldig Bestand opslaan en laden → data blijft gelijk
- Ongeldig pad → exception
- Ongeldig JSON-bestand → exception
