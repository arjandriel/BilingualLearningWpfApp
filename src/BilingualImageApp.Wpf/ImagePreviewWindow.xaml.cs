using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BilingualImageApp.Wpf
{
    public partial class ImagePreviewWindow : Window
    {
        private BitmapImage? _bitmap;
        private int _rotation = 0;
        public int Rotation => _rotation;
        public ImagePreviewWindow(BitmapImage? bitmap, int rotation)
        {
            InitializeComponent();
            if (bitmap != null)
            {
                _bitmap = bitmap;
                _rotation = rotation;
                UpdatePreview();
            }
        }
        private void BtnRotate_Click(object sender, RoutedEventArgs e)
        {
            if (_bitmap == null) return;
            _rotation = (_rotation + 90) % 360;
            UpdatePreview();
        }
        private void UpdatePreview()
        {
            if (_bitmap == null)
            {
                imgPreview.Source = null;
                return;
            }
            if (_rotation == 0)
                imgPreview.Source = _bitmap;
            else
                imgPreview.Source = new TransformedBitmap(_bitmap, new System.Windows.Media.RotateTransform(_rotation));
        }
    }
}