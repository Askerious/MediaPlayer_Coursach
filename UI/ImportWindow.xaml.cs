using Domain;
using Microsoft.Win32;
using System;
using System.CodeDom;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLib;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace UI
{
    public partial class ImportWindow : Window
    {

        //public AudioTrack track;
        public List<AudioTrack> tracks = new List<AudioTrack>();

        public ImportWindow()
        {
            InitializeComponent();
            ImportBox.Items.Add("Файл");
            ImportBox.Items.Add("Все файлы в папке");
            ImportBox.Items.Add("Плейлист");
        }

        public static List<AudioTrack> Import()   
        {
            var window = new ImportWindow();
            return window.ShowDialog() == true ? window.tracks : null;
        }

        public void Browse(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                FileTextBox.Text = dlg.FileName;

                //OpenFileDialog ofd = new OpenFileDialog();
                //ofd.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
                //if (ofd.ShowDialog() == true)
                //    FileTextBox.Text = ofd.FileName;
        }

        public void ImportButton(object sender, RoutedEventArgs e)
        {
            if (FileTextBox.Text == string.Empty) return;

            if (ImportBox.Text == "Файл")
                ImportSingleFile(FileTextBox.Text);
            else if (ImportBox.Text == "Все файлы в папке")
                ImportFolder(FileTextBox.Text);
            else if (FileTextBox.Text == "Плейлист") return;
        }

        public void ImportSingleFile(string path)
        {
            var trackMetadata = File.Create(path);
            tracks.Add(ExtractMetadata(trackMetadata, path));
            DialogResult = true;
            Close();
        }

        public void ImportFolder(string path)
        {
            foreach (var file in System.IO.Directory.GetFiles(path, "*.mp3"))
            {
                var trackMetadata = File.Create(file);
                tracks.Add(ExtractMetadata(trackMetadata, file));
            }
            DialogResult = true;
            Close();
        }

        public void ImportPlaylist(string path)
        {

        }

        public void CreatePlaylist(object sender, RoutedEventArgs e)
        {

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
