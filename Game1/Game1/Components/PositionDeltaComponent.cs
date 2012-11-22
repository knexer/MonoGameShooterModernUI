using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class PositionDeltaComponent : IComponent
    {
        public Vector2 Delta;
        public float RotationDelta = 0;

        public IComponent Clone()
        {
            PositionDeltaComponent ret = new PositionDeltaComponent();
            ret.Delta = Delta;
            ret.RotationDelta = RotationDelta;

            return ret;
        }
    }
}
