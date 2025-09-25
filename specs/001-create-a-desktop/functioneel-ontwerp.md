# Functioneel ontwerp: BilingualImageApp GUI

## Doel
De applicatie stelt gebruikers in staat om afbeeldingen met tweetalige tekst te selecteren, OCR uit te voeren, kolommen aan talen toe te wijzen en tekstparen op te slaan in een bestand.

## Hoofdscherm (MainForm)
- **Menu/Toolbar**
  - Bestand openen (afbeelding selecteren)
  - Bestand opslaan/opslaan als (tekstparenbestand)
  - Afsluiten

- **Afbeeldingselectie**
  - Knop: "Afbeelding kiezen"
  - Toon geselecteerde afbeelding in PictureBox

- **OCR en kolomtoewijzing**
  - Knop: "OCR uitvoeren"
  - Toon herkende tekst in een DataGridView (twee kolommen: Taal 1, Taal 2)
  - Dropdowns of radiobuttons om aan te geven welke kolom welke taal is

- **Tekstparen bewerken**
  - Gebruiker kan tekstparen aanpassen in de DataGridView
  - Knop: "Tekstpaar toevoegen/verwijderen"

- **Opslaan**
  - Knop: "Opslaan" (tekstparenbestand)
  - Knop: "Nieuw bestand" (leeg tekstparenbestand aanmaken)

- **Statusbalk**
  - Meldingen over voortgang, fouten, of succes

## Gebruikersflow
1. Start applicatie
2. Kies een afbeelding
3. Voer OCR uit
4. Wijs kolommen toe aan talen
5. Bewerk/controleer tekstparen
6. Sla tekstparen op in bestand

## Foutafhandeling
- Meldingen bij ontbrekende tessdata, OCR-fouten, of bestandsproblemen
- Validatie op dubbele of incomplete tekstparen

## Toekomstige uitbreidingen (optioneel)
- Ondersteuning voor meerdere afbeeldingen tegelijk
- Undo/redo functionaliteit
- Exporteren naar andere formaten (CSV, Excel)

---

Dit ontwerp vormt de basis voor de implementatie van de WinForms GUI en de koppeling met de bestaande services.