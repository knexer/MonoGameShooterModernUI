using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class HealthComponent : IComponent
    {
        public int Health;

        public IComponent Clone()
        {
            HealthComponent ret = new HealthComponent();
            ret.Health = Health;

            return ret;
        }
    }
}
