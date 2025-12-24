using Data.InMemory;
using Data.Interfaces;
using Data.SqlServer;
using Domain;
using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
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
        public IAudioTrackRepository _trackRepository;
        public IPlaylistRepository _playlistRepository;
        public IUserRepository _userRepository;
        public User _user;
        public MediaDbContext db;

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

        private string _duration;
        public string Duration
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

        public MainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;

            var app = (App)Application.Current;
            _playlistRepository = app._playlistRepository;
            _trackRepository = app._trackRepository;
            _userRepository = app._userRepository;
            _user = user;
            db = app._dbContext;

            if (!db.Playlists.Any(p => (p.Name == "All" && p.UserId == _user.Id)) || !db.Playlists.Any(p => (p.Name == "Favorites" && p.UserId == _user.Id)))
            {
                var all = new Playlist(_user.Id, "All");
                var fav = new Playlist(_user.Id, "Favorites");
                _playlistRepository.Add(all);
                _playlistRepository.Add(fav);
            }

            Playlists = _playlistRepository.GetAll().Where(u => u.UserId == _user.Id).ToList();
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
            {
                string min = Math.Floor(player.NaturalDuration.TimeSpan.TotalMinutes).ToString();
                string sec = Math.Floor(player.NaturalDuration.TimeSpan.TotalSeconds % 60).ToString();
                Duration = $"{min}:{sec}";
            }
            else
                Duration = "0";
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
                UpdateCover(track);
        }

        public void UpdateCover(AudioTrack track)
        {
            nowPlayingArtist.Text = track.Artist;
            nowPlayingTitle.Text = track.Title;

            var cover = new BitmapImage();
            using (var ms = new MemoryStream(track.Cover))
            {
                cover.BeginInit();
                cover.CacheOption = BitmapCacheOption.OnLoad;
                cover.StreamSource = ms;
                cover.EndInit();
                cover.Freeze();
            }

            nowPlayingCover.Source = cover;
        }

        public void LoadTrack(object sender, RoutedEventArgs e)
        {
            List<AudioTrack> tracks = ImportWindow.Import(_user.Id);
            if (tracks != null)
            {
                var all = Playlists.First(p => p.Name == "All");
                foreach (AudioTrack track in tracks)
                {
                    _trackRepository.Add(track);
                    _playlistRepository.AddTrackToPlaylist(all.Id, track);

                    if (_selectedPlaylist != all)
                        _selectedPlaylist.Tracks.Add(track);
                }
            }
            RefreshGrid();
        }

        public void CreatePlaylist(object sender, RoutedEventArgs e)
        {
            Playlist res = CreatePlaylistWindow.CreatePlaylist(Playlists.First(p => p.Name == "All").GetAllTracks(), _user);
            if (res != null)
                _playlistRepository.Add(res);
            RefreshBox();
        }

        public void DeleteTrack(object sender, RoutedEventArgs e)
        {
            var Menu = sender as MenuItem;
            AudioTrack track = null;

            if (Menu != null)
                track = Menu.CommandParameter as AudioTrack;

            if (track != null)
            {
                _selectedPlaylist.Tracks.Remove(track);
                RefreshGrid();
            }
        }

        public void AddToFavorites(object sender, RoutedEventArgs e)
        {
            var Menu = sender as MenuItem;
            AudioTrack track = null;

            if (Menu != null)
                track = Menu.CommandParameter as AudioTrack;

            if (track != null)
            {
                Playlist fav = Playlists.FirstOrDefault(p => p.Name == "Favorites");
                _playlistRepository.AddTrackToPlaylist(fav.Id, track);
                RefreshGrid();
            }
        }

        public void AddToPlaylist(object sender, RoutedEventArgs e)
        {

        }

        public void DeletePlaylist(object sender, RoutedEventArgs e)
        {
            if (_selectedPlaylist == null || _selectedPlaylist.Name == "All" || _selectedPlaylist.Name == "Favorites")
            {
                MessageBox.Show("Системные плейлисты нельзя удалить");
                return;
            }

            var dlg = MessageBox.Show("Удалить плейлист?", "Потверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (dlg == MessageBoxResult.Yes)
            {
                _playlistRepository.RemovePlaylist(_selectedPlaylist.Id);
                _selectedPlaylist = Playlists.First(p => p.Name == "All");
            }

            RefreshGrid();
            RefreshBox();
        }

        public void RefreshGrid()
        {
            if (_selectedPlaylist != null)
                trackListBox.ItemsSource = _selectedPlaylist.GetAllTracks();
        }

        public void RefreshBox()
        {
            Playlists = _playlistRepository.GetAll();
        }
    }
}
