using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AudioTrack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public bool IsFavorite { get; set; }
        public string FilePath { get; set; }

        public AudioTrack (int id, string title, int duration, bool isFavorite, string filePath)
        {
            Id = id;
            Title = title;
            Duration = duration;
            IsFavorite = isFavorite;
            FilePath = filePath;
        }
    }
}
