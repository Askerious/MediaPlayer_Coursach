using Domain;
using Domain.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAudioTrackRepository
    {
        void Add(AudioTrack track);
        List<AudioTrack> GetAll();
    }
}
