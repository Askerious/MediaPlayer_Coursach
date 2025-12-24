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

        public List<Playlist> Playlists { get; set; }   
        public List<AudioTrack> Tracks { get; set; }

        public User(int id, string username, string password, List<Playlist> playlists, List<AudioTrack> tracks)
        {
            Id = id;
            Username = username;
            Password = password;
            Playlists = playlists;
            Tracks = tracks;
        }
    }
}
