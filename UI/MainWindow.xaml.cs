using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
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
using Domain;

namespace UI
{
    public partial class MainWindow : Window
    {
        public MediaPlayer player = new MediaPlayer();
        public ObservableCollection<AudioTrack> tracks = new ObservableCollection<AudioTrack>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void PlayMedia(object sender, EventArgs e)
        {
            player.Play();


        }

        public void LoadTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                player.Open(new Uri(ofd.FileName));
                AudioTrack track = new AudioTrack(1, "test", 45, false, "gavno");
                tracks.Add(track);
                dataGrid.ItemsSource = tracks;
            }
        }
    }
}
