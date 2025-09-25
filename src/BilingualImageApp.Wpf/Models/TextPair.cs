namespace BilingualImageApp.Wpf.Models
{
    /// <summary>
    /// Represents a bilingual text pair extracted from an image.
    /// </summary>
    public class TextPair
    {
        public string? Id { get; set; } // Unique identifier
        public string? Language1 { get; set; }
        public string? Language2 { get; set; }
        public string? SourceImage { get; set; } // Path or name of the image
        public double FuzzyScore { get; set; } = 100.0;
    }
}
