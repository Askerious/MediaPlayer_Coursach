using Data.Interfaces;
using Domain;
using Domain.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Data.InMemory
{
    public class AudioTrackRepository : IAudioTrackRepository
    {
        private List<AudioTrack> _tracks;
        public AudioTrackRepository() => _tracks = new List<AudioTrack>();

        public int GetNextId()
        {
            int id = 0;
            foreach (var tr in _tracks)
                id = Math.Max(id, tr.Id) + 1;
            return id;
        }

        public void Add(AudioTrack track)
        {
            var newTrack= new AudioTrack(track.Title, track.Artist, track.Cover, track.Duration, track.IsFavorite, track.FilePath);
            newTrack.Id = GetNextId();
            _tracks.Add(newTrack);
        }

        public List<AudioTrack> GetAll(TrackFilter filter)
        {
            /*
            var res = _tracks.AsEnumerable();

            if (filter.StartDate.HasValue)
                res = res.Where(r => r.Date >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                res = res.Where(r => r.Date <= filter.EndDate.Value);

            return res.ToList();*/

            return _tracks.ToList();
        }

        public List<AudioTrack> GetAll()
        {
            return _tracks.ToList();
        }
    }
}
