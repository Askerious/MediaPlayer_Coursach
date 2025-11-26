using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Data.Interfaces
{
    public interface IPlaylistRepository
    {
        void Add(Playlist p);
        List<Playlist> GetAll();
    }
}
