using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class LinearMovementComponent : IComponent
    {
        public float RotationRate = 0;

        public IComponent Clone()
        {
            LinearMovementComponent ret = new LinearMovementComponent();
            ret.RotationRate = RotationRate;
            return ret;
        }
    }
}
