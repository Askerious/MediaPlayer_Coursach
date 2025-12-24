using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool isPayed { get; set; }
        public Credentials Credentials { get; set; }

        public List<Playlist> Playlists { get; set; }   
        public List<AudioTrack> Tracks { get; set; }
        
        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Playlists = new List<Playlist>();
            Tracks = new List<AudioTrack>();
        }
    }
}
