using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Playlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name {  get; set; }
        public List<AudioTrack> Tracks { get; set; }

        public Playlist(int userId, string name)
        {
            UserId = userId;
            Name = name;
            Tracks = new List<AudioTrack>();
        }

        public List<AudioTrack> GetAllTracks() => Tracks.ToList(); 
    }
}
