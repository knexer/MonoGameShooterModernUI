using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class DamageOnContactComponent : IComponent
    {
        public int Damage;

        public IComponent Clone()
        {
            DamageOnContactComponent ret = new DamageOnContactComponent();
            ret.Damage = Damage;

            return ret;
        }
    }
}
