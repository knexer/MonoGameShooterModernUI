using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class MoveSpeedComponent : IComponent
    {
        public float MoveSpeed;

        public IComponent Clone()
        {
            MoveSpeedComponent ret = new MoveSpeedComponent();
            ret.MoveSpeed = MoveSpeed;

            return ret;
        }
    }
}
