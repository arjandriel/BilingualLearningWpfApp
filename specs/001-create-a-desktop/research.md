# Phase 0: Research

## Unknowns & Clarifications
- OCR library keuze: Tesseract-OCR via Tesseract .NET wrapper
- Beste praktijk voor fuzzy matching van tekstparen: FuzzySharp
- Minimale GUI toolkit: WPF gekozen (WinForms was initieel, nu gemigreerd)

## Research Tasks
- Vergelijk Tesseract .NET wrappers (Tesseract4Net, Tesseract-OCR, IronOCR) op offline support, licentie, eenvoud. → Tesseract-OCR gekozen, open source, breed gebruikt.
- Zoek naar C# libraries of algoritmen voor fuzzy string matching (bijv. Levenshtein, FuzzySharp). → FuzzySharp gekozen, eenvoudig, open source.
- Vergelijk WinForms, WPF en MAUI voor minimale dependencies en eenvoudige distributie. → WPF gekozen: moderner dan WinForms, geen extra runtime nodig zoals MAUI.
- Onderzoek binding van DataGrid aan tekstpaarmodel: ObservableCollection<Tekstpaar> als ItemsSource.
- Onderzoek veilige file dialogs en error handling in WPF: Microsoft.Win32.OpenFileDialog/SaveFileDialog.
- Integratie bestaande OCR- en opslagservices: direct hergebruiken.

## Beslissingen
- OCR: Tesseract-OCR (open source, offline, breed ondersteund)
- Fuzzy matching: FuzzySharp (open source, eenvoudig)
- GUI: WPF (moderner, betere databinding, geen extra runtime)
- DataGrid binding: ObservableCollection<Tekstpaar>
- File dialogs: Microsoft.Win32.OpenFileDialog/SaveFileDialog
- Services: Hergebruik bestaande OcrService en BestandOpslagService

## Rationale
- Alleen offline, lokaal werkende oplossingen toegestaan.
- Zo min mogelijk externe dependencies, alleen als noodzakelijk voor OCR of matching.
- Eenvoudige distributie en installatie voor eindgebruiker.
- WPF biedt betere databinding en modernere UI dan WinForms.
- Hergebruik van bestaande services minimaliseert onderhoud en bugs.

## Alternatives considered
- Cloud OCR (afgewezen: vereist internet, privacy)
- Zware frameworks (afgewezen: te veel dependencies)
- MAUI (afgewezen: extra runtime, overkill voor desktop-only)
- Eigen fuzzy matching (afgewezen: FuzzySharp is eenvoudiger)

