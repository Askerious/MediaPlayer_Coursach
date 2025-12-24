using Data.Interfaces;
using Domain;
using Domain.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Data.SqlServer
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MediaDbContext _ctx;
        public PlaylistRepository(MediaDbContext ctx) => _ctx = ctx;

        public List<Playlist> GetAll() => _ctx.Playlists.Include(p => p.Tracks).ToList();
        public Playlist GetById(int id) => _ctx.Playlists.Include(p => p.Tracks).FirstOrDefault(p => p.Id == id);

        public void Add(Playlist playlist)
        {
            _ctx.Playlists.Add(playlist);
            _ctx.SaveChanges();
        }

        public void AddTrackToPlaylist(int id, AudioTrack track)
        {
            var playlist = _ctx.Playlists.Include(p => p.Tracks).FirstOrDefault(p => p.Id == id);
            playlist.Tracks.Add(track);
            _ctx.SaveChanges();
        }

        public void RemovePlaylist(int id)
        {
            var ent = _ctx.Playlists.Find(id);

            _ctx.Playlists.Remove(ent);
            _ctx.SaveChanges();
        }

        public void Update(Playlist playlist)
        {
            _ctx.Playlists.Update(playlist);
            _ctx.SaveChanges();
        }
    }
}
