using Data.InMemory;
using Data.Interfaces;
using Domain;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using TagLib;

namespace UI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //public IAudioTrackRepository _trackRepository = new AudioTrackRepository();
        public IPlaylistRepository _playlistRepository = new PlaylistRepository();

        public MediaPlayer player = new MediaPlayer();
        public DispatcherTimer _timer;
        public bool isPlaying = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public double Position
        {
            get => player.Position.TotalSeconds;
            set
            {
                player.Position = TimeSpan.FromSeconds(value);
                OnPropertyChanged();
            }
        }

        private double _duration;
        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public double VolumePercent
        {
            get => player.Volume * 100;
            set
            {
                player.Volume = Math.Max(0, Math.Min(100, value)) / 100.0;
                OnPropertyChanged();
            }
        }

        private Playlist _selectedPlaylist;
        public Playlist SelectedPlaylist
        {
            get => _selectedPlaylist;
            set
            {
                _selectedPlaylist = value;
                OnPropertyChanged();
                RefreshGrid();
            }
        }

        public List<Playlist> _playlists;
        public List<Playlist> Playlists
        {
            get => _playlists;
            set
            {
                _playlists = value;
                OnPropertyChanged();
                RefreshGrid();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var all = new Playlist(12, "All");
            var fav = new Playlist(12, "Favorites");

            _playlistRepository.Add(all);
            _playlistRepository.Add(fav);
            Playlists = new List<Playlist>(_playlistRepository.GetAll());
            SelectedPlaylist = Playlists.First(p => p.Name == "All");

            player.MediaOpened += Player_MediaOpened;
            player.MediaEnded += Player_MediaEnded;
            player.Volume = VolumePercent / 100.0;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += TimerTick;
            _timer.Start();

            RefreshGrid();
        }

        public void Player_MediaOpened(object sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
                Duration = player.NaturalDuration.TimeSpan.TotalSeconds;
            else
                Duration = 0;
        }
        
        public void Player_MediaEnded(object sender, EventArgs e)
        {
            player.Stop();
            Position = 0;
        }

        public void TimerTick(object sender, EventArgs e)
        {
            if (player.NaturalDuration.HasTimeSpan)
                Position = player.Position.TotalSeconds;
        }

        public void PlayMedia(object sender, EventArgs e)
        {
            if (!(trackListBox.SelectedItem is AudioTrack track)) return;

            if (!isPlaying)
            {
                if (player.Source == null || !player.Source.OriginalString.Equals(track.FilePath))
                {
                    player.Stop();
                    player.Open(new Uri(track.FilePath));
                }

                player.Play();
                UpdateCover(track);
                isPlaying = true;
                PlayPauseButton.Content = "Pause";
            }
            else
            {
                player.Pause();
                isPlaying = false;
                PlayPauseButton.Content = "Play";
            }
        }

        public void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (trackListBox.SelectedItem is AudioTrack track)
            {
                UpdateCover(track);
            }
        }

        public void UpdateCover(AudioTrack track)
        {
            nowPlayingArtist.Text = track.Artist;
            nowPlayingTitle.Text = track.Title;
            nowPlayingCover.Source = track.Cover;
        }

        //importWindow
        public void LoadTrack(object sender, RoutedEventArgs e)
        {
            //AudioTrack track = ImportWindow.Import();
            List<AudioTrack> tracks = ImportWindow.Import();
            if (tracks != null)
            {
                var all = _playlistRepository.GetAll().First(p => p.Name == "All");
                foreach (AudioTrack track in tracks)
                {
                    all.Tracks.Add(track);
                    if (_selectedPlaylist != all)
                        _selectedPlaylist.Tracks.Add(track);
                }
            }
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            if (_selectedPlaylist != null)
                trackListBox.ItemsSource = _selectedPlaylist.GetAllTracks();
        }
    }
}
