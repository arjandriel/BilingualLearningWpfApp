using System;

namespace BilingualImageApp.Wpf.Models
{
    /// <summary>
    /// Represents an image and its OCR recognition status.
    /// </summary>
    public class Image
    {
        public string? Id { get; set; } // Path or name of the image
        public DateTime DateAdded { get; set; }
        public RecognitionStatus RecognitionStatus { get; set; }
    }

    /// <summary>
    /// Recognition status for an image.
    /// </summary>
    public enum RecognitionStatus
    {
        Recognized,
        NoText,
        Error
    }
}
