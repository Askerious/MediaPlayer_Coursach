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
        public byte[] Cover { get; set; }
        public int Duration { get; set; }
        public bool IsFavorite { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; } = null;

        public AudioTrack() { }
        public AudioTrack(string title, string artist, byte[] cover, int duration, bool isFavorite, string filePath, int userId)
        {
            Title = title;
            Artist = artist;
            Cover = cover;
            Duration = duration;
            IsFavorite = isFavorite;
            FilePath = filePath;
            Date = DateTime.Now;
            UserId = userId;
        }
    }
}
