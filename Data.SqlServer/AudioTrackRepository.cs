using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SqlServer
{
    public class AudioTrackRepository : IAudioTrackRepository
    {
        private readonly MediaDbContext _ctx;
        public AudioTrackRepository(MediaDbContext ctx) => _ctx = ctx;

        public List<AudioTrack> GetAll() => _ctx.Tracks.AsQueryable().ToList();
        public AudioTrack GetById(Guid id) => _ctx.Tracks.Find(id);

        public void Add(AudioTrack track)
        {
            _ctx.Tracks.Add(track);
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var ent = _ctx.Tracks.Find(id);
            if (ent == null) return;

            _ctx.Tracks.Remove(ent);
            _ctx.SaveChanges();
        }

        public void Update(AudioTrack track)
        {
            _ctx.Tracks.Update(track);
            _ctx.SaveChanges();
        }
    }
}
