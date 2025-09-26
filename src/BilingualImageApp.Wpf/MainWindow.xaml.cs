using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using BilingualImageApp.Wpf.Models;
using BilingualImageApp.Wpf.Services;
using Tesseract;

namespace BilingualImageApp.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<TextPair> _textPairs = new();
    private string? _selectedImagePath;
    private BitmapImage? _currentBitmap;
    private int _currentRotation = 0;
    private readonly OcrService _ocrService;
    private readonly FuzzyMatchService _fuzzyService;
    private readonly string _tessdataPath = "./tessdata"; // Adjust as needed

    public MainWindow()
    {
        InitializeComponent();
        dataGridTextPairs.ItemsSource = _textPairs;
        _ocrService = new OcrService(_tessdataPath);
        _fuzzyService = new FuzzyMatchService();
        btnOcr.IsEnabled = false;
    }

    private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog { Filter = "Afbeeldingen (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dlg.ShowDialog() == true)
        {
            _selectedImagePath = dlg.FileName;
            txtSelectedImage.Text = System.IO.Path.GetFileName(_selectedImagePath);
            _currentBitmap = new BitmapImage();
            _currentBitmap.BeginInit();
            _currentBitmap.UriSource = new Uri(_selectedImagePath);
            _currentBitmap.CacheOption = BitmapCacheOption.OnLoad;
            _currentBitmap.Rotation = Rotation.Rotate0;
            _currentBitmap.EndInit();
            _currentRotation = 0;
            // Open the image preview popup immediately
            var dlgPreview = new ImagePreviewWindow(_currentBitmap, _currentRotation);
            dlgPreview.Owner = this;
            dlgPreview.ShowDialog();
            _currentRotation = dlgPreview.Rotation;
            btnOcr.IsEnabled = true;
        }
    }


    private void BtnPreview_Click(object sender, RoutedEventArgs e)
    {
        if (_currentBitmap == null) return;
        var dlg = new ImagePreviewWindow(_currentBitmap, _currentRotation);
        dlg.Owner = this;
        dlg.ShowDialog();
        // Update rotation if changed in dialog
        _currentRotation = dlg.Rotation;
    }

    private void BtnOcr_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedImagePath) || !File.Exists(_selectedImagePath) || _currentBitmap == null)
        {
            txtStatus.Text = "Geen afbeelding geselecteerd.";
            return;
        }
        try
        {
            // Apply rotation if needed
            BitmapSource bmpSource = _currentBitmap;
            if (_currentRotation != 0)
            {
                bmpSource = new TransformedBitmap(_currentBitmap, new System.Windows.Media.RotateTransform(_currentRotation));
            }
            // Convert BitmapSource to Pix
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmpSource));
            using var ms = new MemoryStream();
            encoder.Save(ms);
            ms.Position = 0;
            var pix = Pix.LoadFromMemory(ms.ToArray());
            var ocrText = _ocrService.ExtractText(pix);
            // (No popup: raw OCR output display removed; was for debugging)

            var lines = ocrText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            _textPairs.Clear();
            int maxColumns = 0;
            foreach (var line in lines)
            {
                string[] columns;
                if (line.Contains("O"))
                {
                    // Split on 'O' with optional spaces around
                    columns = System.Text.RegularExpressions.Regex.Split(line, @"\s*O\s*");
                }
                else
                {
                    // Fallback: split by tab or 2+ spaces
                    columns = System.Text.RegularExpressions.Regex.Split(line, "[\t]|[ ]{2,}");
                }
                // Trim and filter empty parts
                var parts = columns.Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToArray();
                if (parts.Length >= 2)
                {
                    // Skip header lines
                    if (parts[0].Equals("Frans", StringComparison.OrdinalIgnoreCase) || parts[0].Equals("Frans > Nederlands", StringComparison.OrdinalIgnoreCase))
                        continue;
                    // Add as a pair
                    _textPairs.Add(new TextPair
                    {
                        Language1 = parts[0],
                        Language2 = parts[1],
                        SourceImage = System.IO.Path.GetFileName(_selectedImagePath)
                    });
                }
            }
            // If no pairs found, offer to save raw OCR output
            if (_textPairs.Count == 0)
            {
                var result = MessageBox.Show(
                    "Er zijn geen tekstparen gevonden. Wilt u de ruwe OCR-uitvoer opslaan voor analyse?",
                    "Geen tekstparen gevonden",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var saveDlg = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "Tekstbestand (*.txt)|*.txt",
                        FileName = "ocr_raw_output.txt"
                    };
                    if (saveDlg.ShowDialog() == true)
                    {
                        try
                        {
                            File.WriteAllText(saveDlg.FileName, ocrText);
                        }
                        catch (Exception exFile)
                        {
                            MessageBox.Show($"Kon OCR-tekst niet naar bestand schrijven: {exFile.Message}", "Fout bij schrijven");
                        }
                    }
                }
            }
            MessageBox.Show($"Detected {maxColumns} columns per line (max). Text pairs created: {_textPairs.Count}", "Column Detection");
            txtStatus.Text = $"{_textPairs.Count} tekstparen gevonden.";
        }
        catch (Exception ex)
        {
            txtStatus.Text = "Fout bij OCR: " + ex.Message;
        }
    }

    private Pix BitmapImageToPix(BitmapImage bitmapImage)
    {
        using var ms = new MemoryStream();
        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
        encoder.Save(ms);
        ms.Position = 0;
        return Pix.LoadFromMemory(ms.ToArray());
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new SaveFileDialog { Filter = "CSV-bestand (*.csv)|*.csv" };
        if (dlg.ShowDialog() == true)
        {
            try
            {
                using var writer = new StreamWriter(dlg.FileName);
                writer.WriteLine("Language1,Language2,SourceImage,FuzzyScore");
                foreach (var pair in _textPairs)
                {
                    // Escape commas and quotes for CSV
                    string l1 = EscapeCsv(pair.Language1);
                    string l2 = EscapeCsv(pair.Language2);
                    string img = EscapeCsv(pair.SourceImage);
                    string score = pair.FuzzyScore.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    writer.WriteLine($"{l1},{l2},{img},{score}");
                }
                txtStatus.Text = "Opgeslagen als CSV.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fout bij opslaan: " + ex.Message;
            }
        }
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog { Filter = "CSV-bestand (*.csv)|*.csv" };
        if (dlg.ShowDialog() == true)
        {
            try
            {
                _textPairs.Clear();
                using var reader = new StreamReader(dlg.FileName);
                string? header = reader.ReadLine(); // skip header
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var columns = ParseCsvLine(line);
                    if (columns.Length >= 4)
                    {
                        _textPairs.Add(new TextPair
                        {
                            Language1 = columns[0],
                            Language2 = columns[1],
                            SourceImage = columns[2],
                            FuzzyScore = double.TryParse(columns[3], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var score) ? score : 100.0
                        });
                    }
                }
                txtStatus.Text = "CSV-bestand geladen.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fout bij laden: " + ex.Message;
            }
        }
    }
    // Helper to escape CSV fields
    private static string EscapeCsv(string? field)
    {
        if (field == null) return "";
        if (field.Contains('"')) field = field.Replace("\"", "\"\"");
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            return $"\"{field}\"";
        return field;
    }

    // Helper to parse a CSV line (simple, not RFC4180-complete)
    private static string[] ParseCsvLine(string line)
    {
        var result = new System.Collections.Generic.List<string>();
        bool inQuotes = false;
        var value = new System.Text.StringBuilder();
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        value.Append('"');
                        i++;
                    }
                    else
                        inQuotes = false;
                }
                else
                    value.Append(c);
            }
            else
            {
                if (c == ',')
                {
                    result.Add(value.ToString());
                    value.Clear();
                }
                else if (c == '"')
                    inQuotes = true;
                else
                    value.Append(c);
            }
        }
        result.Add(value.ToString());
        return result.ToArray();
    }
}