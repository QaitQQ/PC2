using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{

    public partial class SiteItemGenerator
    {
        sealed class WrapImage : INotifyPropertyChanged
        {
            private BitmapImage _Image;
            private System.Drawing.Image img;
            public WrapImage(BitmapImage image) { _Image = image; }
            public WrapImage(System.Drawing.Image image)
            {
                if (image != null)
                {
                    _Image = ConvertIMG(image); img = image;
                } 
            }
            public BitmapImage Image
            {
                get { return _Image; }
                set
                {
                    _Image = value;
                    OnPropertyChanged("Image");
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
            private BitmapImage ConvertIMG(System.Drawing.Image img)
            {
                try
                {
                    using MemoryStream memory = new MemoryStream();
                    img.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    memory.Close();
                    return bitmapImage;
                }
                catch
                {
                    return null;
                }
            }

            public System.Drawing.Image GetImage() { return img; }

        }
    }
}
