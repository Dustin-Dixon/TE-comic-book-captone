using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public interface IVolumeDAO
    {
        bool AddVolume(Volume newVolume);
        Volume GetById(int volumeId);
        Volume GetComicVolume(int comicId);
    }
}
