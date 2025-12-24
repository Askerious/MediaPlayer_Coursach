using Domain;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLib;

namespace UI
{
    public partial class ImportWindow : Window
    {

        //public AudioTrack track;
        public List<AudioTrack> tracks = new List<AudioTrack>();
        public static int _userId;

        public ImportWindow()
        {
            InitializeComponent();
            ImportBox.Items.Add("Файл");
            ImportBox.Items.Add("Все файлы в папке");
            ImportBox.Items.Add("Плейлист");
        }

        public static List<AudioTrack> Import(int userId)   
        {
            var window = new ImportWindow();
            _userId = userId;
            return window.ShowDialog() == true ? window.tracks : null;
        }

        public void Browse(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();

            if (ImportBox.Text == "Файл")
                dlg.IsFolderPicker = false;
            else
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
                ImportSingleFile(FileTextBox.Text, _userId);
            else if (ImportBox.Text == "Все файлы в папке")
                ImportFolder(FileTextBox.Text, _userId);
            else if (FileTextBox.Text == "Плейлист") return;
        }

        public void ImportSingleFile(string path, int userId)
        {
            if (!System.IO.File.Exists(path))
            {
                System.Windows.MessageBox.Show("Требуется путь до файла");
                return;
            }

            var trackMetadata = File.Create(path);
            tracks.Add(ExtractMetadata(trackMetadata, path, userId));
            DialogResult = true;
            Close();
        }

        public void ImportFolder(string path, int userId)
        {
            foreach (var file in System.IO.Directory.GetFiles(path, "*.mp3"))
            {
                var trackMetadata = File.Create(file);
                tracks.Add(ExtractMetadata(trackMetadata, file, userId));
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

        public AudioTrack ExtractMetadata(File file, string path, int userId)
        {
            string title = file.Tag.Title;
            string artist = file.Tag.FirstPerformer;
            bool isFavorite = false;

            string min = Math.Floor(file.Properties.Duration.TotalMinutes).ToString();
            string sec = Math.Floor(file.Properties.Duration.TotalSeconds % 60).ToString();
            string duration = $"{min}:{sec}";

            /*
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
            }*/

            byte[] cover = null;
            if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
            {
                var pic = file.Tag.Pictures[0];
                // TagLib.Picture.Data — TagLib.ByteVector; используем ToArray() для безопасности
                cover = pic.Data?.ToArray();
            }

            return new AudioTrack(title, artist, cover, duration, isFavorite, path, userId);
        }
    }
}
