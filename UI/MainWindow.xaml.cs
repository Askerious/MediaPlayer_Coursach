using Data.InMemory;
using Data.Interfaces;
using Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MediaPlayer player = new MediaPlayer();
        public readonly IAudioTrackRepository _trackRepository = new AudioTrackRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void PlayMedia(object sender, EventArgs e)
        {
            if (dataGrid.SelectedItem is AudioTrack track)
            {
                player.Open(new Uri(track.FilePath));
                player.Play();
            }
            
        }

        //importWindow
        public void LoadTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                var trackMetadata = File.Create(ofd.FileName);
                AudioTrack track = ExtractMetadata(trackMetadata, ofd.FileName);
                _trackRepository.Add(track);
                RefreshGrid();
            }
        }

        public AudioTrack ExtractMetadata(File file, string path)
        {
            string title = file.Tag.Title;
            string artist = file.Tag.FirstPerformer;
            double duration = file.Properties.Duration.TotalMinutes;
            bool isFavorite = false;

            return new AudioTrack(title, artist, duration, isFavorite, path);
        }

        public void RefreshGrid()
        {
            dataGrid.ItemsSource = _trackRepository.GetAll();
        }
    }
}
