using Domain;
using System;
using System.Collections.Generic;
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
