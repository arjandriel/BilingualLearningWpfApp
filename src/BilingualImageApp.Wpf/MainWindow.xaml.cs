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
    private readonly OcrService _ocrService;
    private readonly FuzzyMatchService _fuzzyService;
    private readonly string _tessdataPath = "./tessdata"; // Adjust as needed

    public MainWindow()
    {
        InitializeComponent();
        dataGridTextPairs.ItemsSource = _textPairs;
        _ocrService = new OcrService(_tessdataPath);
        _fuzzyService = new FuzzyMatchService();
    }

    private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog { Filter = "Afbeeldingen (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp" };
        if (dlg.ShowDialog() == true)
        {
            _selectedImagePath = dlg.FileName;
            txtSelectedImage.Text = System.IO.Path.GetFileName(_selectedImagePath);
        }
    }

    private void BtnOcr_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedImagePath) || !File.Exists(_selectedImagePath))
        {
            txtStatus.Text = "Geen afbeelding geselecteerd.";
            return;
        }
        try
        {
            var bmpImage = new BitmapImage(new Uri(_selectedImagePath));
            var pix = BitmapImageToPix(bmpImage);
            var ocrText = _ocrService.ExtractText(pix);
            var lines = ocrText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            _textPairs.Clear();
            foreach (var line in lines)
            {
                var columns = line.Split('\t');
                if (columns.Length >= 2)
                {
                    _textPairs.Add(new TextPair
                    {
                        Language1 = columns[0],
                        Language2 = columns[1],
                        SourceImage = System.IO.Path.GetFileName(_selectedImagePath)
                    });
                }
            }
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