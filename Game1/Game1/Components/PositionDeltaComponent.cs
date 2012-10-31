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

        public IComponent Clone()
        {
            PositionDeltaComponent ret = new PositionDeltaComponent();
            ret.Delta = Delta;

            return ret;
        }
    }
}
