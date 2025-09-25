using System;
using System.IO;
using Tesseract;

namespace BilingualImageApp.Wpf.Services
{
    /// <summary>
    /// Service for extracting text from an image using Tesseract OCR.
    /// </summary>
    public class OcrService
    {
        private readonly string _tessdataPath;
        private readonly string _language;

        public OcrService(string tessdataPath, string language = "eng")
        {
            _tessdataPath = tessdataPath;
            _language = language;
        }

        /// <summary>
        /// Extracts text from a Pix image. Throws on error or missing tessdata.
        /// </summary>
        public string ExtractText(Pix pix)
        {
            try
            {
                using var engine = new TesseractEngine(_tessdataPath, _language, EngineMode.Default);
                using var page = engine.Process(pix);
                return page.GetText();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("OCR failed: " + ex.Message, ex);
            }
        }
    }
}
