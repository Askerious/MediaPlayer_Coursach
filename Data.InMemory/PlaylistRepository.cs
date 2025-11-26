using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.InMemory
{
    public class PlaylistRepository : IPlaylistRepository
    {
        public List<Playlist> _playlists;
        public PlaylistRepository() => _playlists = new List<Playlist>();

        public int GetNextId()
        {
            int id = 0;
            foreach (var tr in _playlists)
                id = Math.Max(id, tr.Id) + 1;
            return id;
        }

        public void Add(Playlist playlist)
        {
            var newPlaylist = new Playlist(playlist.UserId, playlist.Name);
            newPlaylist.Id = GetNextId();
            _playlists.Add(newPlaylist);
        }

        public List<Playlist> GetAll() => _playlists;
    }
}
