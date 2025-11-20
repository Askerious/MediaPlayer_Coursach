using Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLib;

namespace UI
{
    public partial class ImportWindow : Window
    {

        public AudioTrack track;

        public ImportWindow()
        {
            InitializeComponent();
        }

        public static AudioTrack Import()
        {
            var window = new ImportWindow();
            return window.ShowDialog() == true ? window.track : null;
        }

        public void Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
                FileTextBox.Text = ofd.FileName;
        }

        public void ImportButton(object sender, RoutedEventArgs e)
        {
            var trackMetadata = File.Create(FileTextBox.Text);
            track = ExtractMetadata(trackMetadata, FileTextBox.Text);
            DialogResult = true;
            Close();
        }

        public AudioTrack ExtractMetadata(File file, string path)
        {
            string title = file.Tag.Title;
            string artist = file.Tag.FirstPerformer;
            int duration = (int)file.Properties.Duration.TotalSeconds;
            bool isFavorite = false;

            BitmapImage cover = null;
            if (file.Tag.Pictures.Length > 0)
            {
                var pic = file.Tag.Pictures[0];
                using (var ms = new System.IO.MemoryStream(pic.Data.Data))
                {
                    cover = new BitmapImage();
                    cover.BeginInit();
                    cover.CacheOption = BitmapCacheOption.OnLoad;
                    cover.StreamSource = ms;
                    cover.EndInit();
                }
            }

            return new AudioTrack(title, artist, cover, duration, isFavorite, path);
        }
    }
}
