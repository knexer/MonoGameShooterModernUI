using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class SoundEffectComponent : IComponent
    {
        public SoundEffect effect;

        public IComponent Clone()
        {
            SoundEffectComponent ret = new SoundEffectComponent();
            ret.effect = effect;

            return ret;
        }
    }
}
