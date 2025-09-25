using FuzzySharp;

namespace BilingualImageApp.Wpf.Services
{
    /// <summary>
    /// Service for fuzzy matching of text pairs.
    /// </summary>
    public class FuzzyMatchService
    {
        /// <summary>
        /// Determines if two strings are duplicates based on fuzzy ratio and threshold.
        /// </summary>
        public bool IsDuplicate(string text1, string text2, double threshold = 90.0)
        {
            if (string.IsNullOrWhiteSpace(text1) || string.IsNullOrWhiteSpace(text2))
                return false;
            var score = Fuzz.Ratio(text1, text2);
            return score >= threshold;
        }
    }
}
