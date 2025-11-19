using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Domain
{
    public class AudioTrack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public double Duration { get; set; }
        public bool IsFavorite { get; set; }
        public string FilePath { get; set; }

        public AudioTrack (string title, string artist, double duration, bool isFavorite, string filePath)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            IsFavorite = isFavorite;
            FilePath = filePath;
        }
    }
}
