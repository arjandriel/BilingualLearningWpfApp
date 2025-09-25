using System.Collections.Generic;

namespace BilingualImageApp.Wpf.Models
{
    /// <summary>
    /// Represents a document containing text pairs and images.
    /// </summary>
    public class Document
    {
        public string? Id { get; set; } // File name
        public List<TextPair> TextPairs { get; set; } = new List<TextPair>();
        public List<Image> Images { get; set; } = new List<Image>();
    }
}
