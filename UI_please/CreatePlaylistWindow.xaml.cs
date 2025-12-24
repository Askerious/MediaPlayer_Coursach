using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    public partial class CreatePlaylistWindow : Window
    {
        public Playlist playlist;
        public static List<AudioTrack> _source;
        public static User _user;

        public CreatePlaylistWindow()
        {
            InitializeComponent();
            trackListBox.ItemsSource = _source;
        }

        public static Playlist CreatePlaylist(List<AudioTrack> source, User user)
        {
            _source = source;
            _user = user;
            var window = new CreatePlaylistWindow();
            return window.ShowDialog() == true ? window.playlist : null;
        }

        public void CreatePlaylistButton(object sender, RoutedEventArgs e)
        {
            playlist = new Playlist(_user.Id, PlaylistNameBox.Text);
            foreach (AudioTrack track in trackListBox.SelectedItems)
                playlist.Tracks.Add(track);

            DialogResult = true;
            Close();
        }
    }
}
