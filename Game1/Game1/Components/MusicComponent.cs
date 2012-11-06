using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class MusicComponent : IComponent
    {
        public Song music;
        public bool repeat;

        public IComponent Clone()
        {
            MusicComponent ret = new MusicComponent();
            ret.music = music;
            ret.repeat = repeat;

            return ret;
        }
    }
}
