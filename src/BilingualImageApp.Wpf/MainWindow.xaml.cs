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
        var dlg = new SaveFileDialog { Filter = "JSON-bestand (*.json)|*.json" };
        if (dlg.ShowDialog() == true)
        {
            try
            {
                var doc = new Document
                {
                    Id = System.IO.Path.GetFileName(dlg.FileName),
                    TextPairs = _textPairs.ToList(),
                    Images = new() { new BilingualImageApp.Wpf.Models.Image { Id = _selectedImagePath, DateAdded = DateTime.Now, RecognitionStatus = RecognitionStatus.Recognized } }
                };
                var json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(dlg.FileName, json);
                txtStatus.Text = "Opgeslagen.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fout bij opslaan: " + ex.Message;
            }
        }
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog { Filter = "JSON-bestand (*.json)|*.json" };
        if (dlg.ShowDialog() == true)
        {
            try
            {
                var json = File.ReadAllText(dlg.FileName);
                var doc = JsonSerializer.Deserialize<Document>(json);
                _textPairs.Clear();
                if (doc?.TextPairs != null)
                {
                    foreach (var pair in doc.TextPairs)
                        _textPairs.Add(pair);
                }
                txtStatus.Text = "Bestand geladen.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fout bij laden: " + ex.Message;
            }
        }
    }
}