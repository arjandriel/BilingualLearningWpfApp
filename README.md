# Bilingual Learning WPF App

A Windows desktop application for extracting and storing bilingual text pairs from images. The app features a Dutch user interface and an English codebase. It uses OCR (Tesseract) and fuzzy matching to help users build bilingual datasets from images.

## Features
- Select images containing text in two languages
- Extract text using OCR (Tesseract)
- Assign columns to languages (user-specified)
- Store and load bilingual text pairs in JSON files
- Add new images to existing files or create new files
- Fuzzy duplicate detection
- Dutch-language WPF user interface
- All code in English
- Unit and integration tests (MSTest)

## Quickstart
1. Build the solution in Visual Studio 2022 or later (.NET 10.0-windows required)
2. Run the WPF app from `src/BilingualImageApp.Wpf`
3. Use the UI to select images, extract text, assign columns, and save/load files
4. Run tests with `dotnet test tests/BilingualImageApp.Tests`

## Project Structure
- `src/BilingualImageApp.Wpf/` - Main WPF application (models, services, UI)
- `tests/BilingualImageApp.Tests/` - Unit and integration tests
- `specs/001-create-a-desktop/` - Specifications, contracts, and planning docs

## Dependencies
- Tesseract (OCR)
- FuzzySharp (fuzzy matching)
- System.Text.Json (storage)
- MSTest v4 (testing)

## License
MIT License
