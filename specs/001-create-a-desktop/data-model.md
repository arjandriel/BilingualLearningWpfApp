# Data Model

## Tekstpaar
- Id: string (uniek)
- Taal1: string
- Taal2: string
- BronAfbeelding: string (pad/naam van afbeelding)
- FuzzyScore: double (optioneel, default 100.0)

## Afbeelding
- Id: string (pad/naam)
- DatumToegevoegd: DateTime
- TekstHerkenningStatus: enum { Herkend, GeenTekst, Fout }

## Bestand
- Id: string (bestandsnaam)
- Tekstparen: List<Tekstpaar>
- Afbeeldingen: List<Afbeelding>

## Validatie
- Geen lege tekstparen (Taal1 en Taal2 verplicht)
- Geen dubbele tekstparen (fuzzy match)

## Relaties
- Een Bestand bevat meerdere Tekstparen en Afbeeldingen
- Een Tekstpaar hoort bij één Afbeelding
