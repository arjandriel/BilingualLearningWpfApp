
---

## 📝 Requirements & Lessons Learned (2025-09-26)

### Additional Requirements (found during development)
- **CSV Storage**: Text pairs must be stored in CSV format for easier manual editing. (Replaces JSON storage.)
- **Build Output Path**: All build outputs must be placed in a `Build` directory at the root of the workspace, using `Directory.Build.props`.
- **Robust OCR Parsing**: The OCR logic must handle text pairs separated by bullets, tabs, or spaces, and be resilient to inconsistent formatting.
- **UI Compactness**: Language selection controls (ComboBoxes) must be compact and visually efficient.
- **Popup Image Preview & Rotation**: Selecting an image opens a popup for preview and rotation before OCR is performed.
- **No Debug Popups**: The application does not show the raw OCR result in a popup; only user-initiated actions are shown.
- **Optional Raw OCR Output Logging**: If no text pairs are found, the user must be given the option to save the raw OCR output to a file for troubleshooting and validation.
- **Dutch UI, English Code**: All code, comments, and identifiers are in English; the user interface is fully in Dutch.
- **Test-Driven Development**: All features are developed using TDD, with unit and integration tests for models, services, and UI logic.
- **Minimal Dependencies**: Only essential libraries are used (Tesseract, FuzzySharp, System.Text.Json, System.Drawing.Common, MSTest v4).
- **Error Handling**: The app must provide clear error messages for missing tessdata, OCR failures, file issues, and invalid/duplicate text pairs.
- **Performance**: OCR must complete in under 2 seconds per image; UI must remain responsive.
- **Version Control**: All changes are tracked in Git and pushed to GitHub.

### Lessons Learned
- Output path redirection for WPF must use `Directory.Build.props` (not project file alone).
- OCR output may use various separators (bullets, tabs, spaces); parsing must be flexible.
- UI controls (especially ComboBoxes) require explicit sizing for compactness.
- Immediate image preview/rotation improves user workflow and data quality.
- Debug popups (e.g. raw OCR result) should be removed for production; only user-initiated dialogs remain.
- Logging raw OCR output is essential for debugging extraction issues, but should be user-initiated if no pairs are found.
- CSV is more user-friendly for manual editing than JSON for this use case.

---
# Feature Specification: Desktop app voor tweetalige tekstherkenning uit afbeeldingen

**Feature Branch**: `001-create-a-desktop`  
**Created**: September 25, 2025  
**Status**: Draft  
**Input**: User description: "Create a desktop application in which the user can select an image. On this image there are words/senteces in 2 languages. These need to be read from the image. The user can specify which colom is which language. This text is stored in a file so it can be read later. Info from other images can be added to a file or stored in a new file."

## Execution Flow (main)
```
1. Parse user description from Input
   → If empty: ERROR "No feature description provided"
2. Extract key concepts from description
   → Identify: actors, actions, data, constraints
3. For each unclear aspect:
   → Mark with [NEEDS CLARIFICATION: specific question]
4. Fill User Scenarios & Testing section
   → If no clear user flow: ERROR "Cannot determine user scenarios"
5. Generate Functional Requirements
   → Each requirement must be testable
   → Mark ambiguous requirements
6. Identify Key Entities (if data involved)
8. Return: SUCCESS (spec ready for planning)
```
## ⚡ Quick Guidelines
- ✅ Focus on WHAT users need and WHY
- ❌ Avoid HOW to implement (no tech stack, APIs, code structure)
- 👥 Written for business stakeholders, not developers

### Section Requirements
- **Mandatory sections**: Must be completed for every feature
- **Optional sections**: Include only when relevant to the feature
- When a section doesn't apply, remove it entirely (don't leave as "N/A")

### For AI Generation
When creating this spec from a user prompt:
1. **Mark all ambiguities**: Use [NEEDS CLARIFICATION: specific question] for any assumption you'd need to make
2. **Don't guess**: If the prompt doesn't specify something (e.g., "login system" without auth method), mark it
4. **Common underspecified areas**:
   - User types and permissions
   - Data retention/deletion policies  
   - Performance targets and scale
   - Error handling behaviors
   - Integration requirements
   - Security/compliance needs

---

## Clarifications
### Session 2025-09-25
- Q: Hoe moet het systeem bepalen of twee tekstfragmenten (paren) als duplicaat worden beschouwd? → A: Fuzzy match (kleine verschillen toegestaan)
- Q: Hoe moet het systeem bepalen welke tekst bij welke kolom/taal hoort als de afbeelding niet duidelijk twee kolommen bevat? → A: Systeem probeert automatisch te detecteren, gebruiker kan corrigeren
- Q: Wat moet het systeem doen als een gebruiker de verkeerde kolom aan een taal toewijst en dit pas na opslaan ontdekt? → A: Systeem vraagt altijd om bevestiging voor opslaan
- Q: Wat moet het systeem doen als er geen tekst of geen paren kunnen worden herkend op een afbeelding? → A: Systeem toont een duidelijke foutmelding en slaat niets op

## User Scenarios & Testing *(mandatory)*

### Primary User Story

### Acceptance Scenarios
- Wat gebeurt er als de afbeelding niet duidelijk twee kolommen bevat? → Systeem probeert automatisch te detecteren, gebruiker kan corrigeren
- Hoe gaat het systeem om met afbeeldingen waar geen tekst of geen paren kunnen worden herkend?
- Wat als een gebruiker per ongeluk dezelfde afbeelding meerdere keren toevoegt? → Systeem vraagt gebruiker om bevestiging bij dubbele afbeelding
- Wat als de gebruiker de verkeerde kolom aan een taal toewijst?
- **FR-006**: Gebruiker MOET een nieuw bestand kunnen aanmaken voor een nieuwe set tekstparen.
- **FR-007**: Systeem MOET foutmeldingen geven als geen tekst of geen paren kunnen worden herkend.
- **Afbeelding**: Een bestand met visuele tekst in twee kolommen/talen.
- **Bestand**: Een opslagbestand waarin tekstparen worden opgeslagen, waaraan meerdere afbeeldingen kunnen worden toegevoegd.

---

## Review & Acceptance Checklist

## Review & Acceptance Checklist
- [x] Focused on user value and business needs
- [x] Written for non-technical stakeholders
- [x] Success criteria are measurable
- [x] Dependencies and assumptions identified

---

- [x] Requirements generated
- [x] Entities identified
- [ ] Review checklist passed

---
