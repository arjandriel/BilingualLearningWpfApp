# Takenlijst voor Desktop app tweetalige tekstherkenning

## Setup
T001. Initialiseer C# desktopproject (WPF) in `src/BilingualImageApp.Wpf` [src/BilingualImageApp.Wpf/]
T002. Voeg MSTest testproject toe in `tests/BilingualImageApp.Tests` [tests/BilingualImageApp.Tests/]
T003. Voeg noodzakelijke NuGet packages toe (Tesseract, FuzzySharp, System.Text.Json) [src/BilingualImageApp.Wpf/]

## Modellen en data
T004. Implementeer het `Afbeelding` model [src/BilingualImageApp/Models/Afbeelding.cs]
T005. Implementeer het `Tekstpaar` model [src/BilingualImageApp/Models/Tekstpaar.cs]
T006. Implementeer het `Bestand` model [src/BilingualImageApp/Models/Bestand.cs]

## Test-driven development
T007. Schrijf MSTest unit tests voor alle modellen [tests/BilingualImageApp.Tests/Models/]
T008. Schrijf integratietest voor quickstart scenario (end-to-end) [tests/BilingualImageApp.Tests/Integration/]

## OCR en fuzzy matching
T009. Implementeer OCR service volgens contract (Tesseract) [src/BilingualImageApp/Services/OcrService.cs]
T010. Implementeer fuzzy matching service volgens contract [src/BilingualImageApp/Services/FuzzyMatchService.cs]
T011. Schrijf unittests voor OCR en fuzzy matching [tests/BilingualImageApp.Tests/Services/]

## Kernfunctionaliteit
T012. Implementeer WPF-GUI voor afbeeldingselectie, DataGrid, kolomtoewijzing [src/BilingualImageApp.Wpf/Views/MainWindow.xaml, MainWindow.xaml.cs]
T013. Implementeer logica voor automatisch detecteren en corrigeren van kolommen [src/BilingualImageApp/Services/ColumnDetectService.cs]
T014. Implementeer opslag en laden van tekstparen in bestand volgens contract [src/BilingualImageApp/Services/BestandOpslagService.cs]
T015. Implementeer bevestigingsdialoog bij kolomtoewijzing en dubbele afbeeldingen [src/BilingualImageApp.Wpf/Views/Dialogs/]
T016. Implementeer foutafhandeling en meldingen [src/BilingualImageApp/Services/ErrorService.cs]

## Polish & documentatie
T017. Schrijf gebruikersdocumentatie (README, quickstart) [specs/001-create-a-desktop/]
T018. Optimaliseer performance (OCR < 2s per afbeelding) [src/BilingualImageApp/Services/OcrService.cs]
T019. Controleer en verbeter foutmeldingen en UX [src/BilingualImageApp.Wpf/]
T020. Finaliseer en review codebase [src/, tests/, specs/]

## Dependency- en volgorde-overzicht
- T001, T002, T003 (setup) zijn eerste stap
- T004, T005, T006 (modellen) kunnen parallel na setup
- T007 (model tests) vóór implementatie modellen (TDD)
- T009, T010 (services) na modellen
- T011 (service tests) vóór implementatie services (TDD)
- T012-T016 (kernfunctionaliteit) na modellen en services
- T008 (integratietest) na basisfunctionaliteit
- T017-T020 (polish/documentatie) na basisfunctionaliteit

## Parallelle taken
- Modellen (T004-T006)
- Model tests (T007)
- Service tests (T011)
- Documentatie (T017)

## Acceptatiecriteria
- Alle contracten en data-model zijn geïmplementeerd
- Alle tests (unit/integratie) slagen
- Quickstart-scenario werkt end-to-end
- Gebruikersdocumentatie is aanwezig
- Performance en UX voldoen aan eisen
