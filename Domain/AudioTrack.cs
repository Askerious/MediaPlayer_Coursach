using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows.Media.Imaging;

namespace Domain
{
    public class AudioTrack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public BitmapImage Cover { get; set; }
        public int Duration { get; set; }
        public bool IsFavorite { get; set; }
        public string FilePath { get; set; }

        public AudioTrack(string title, string artist, BitmapImage cover, int duration, bool isFavorite, string filePath)
        {
            Title = title;
            Artist = artist;
            Cover = cover;
            Duration = duration;
            IsFavorite = isFavorite;
            FilePath = filePath;
        }
    }
}
