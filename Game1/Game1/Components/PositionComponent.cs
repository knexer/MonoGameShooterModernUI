using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class PositionComponent : IComponent
    {
        public Vector2 Position;

        public IComponent Clone()
        {
            PositionComponent ret = new PositionComponent();
            ret.Position = Position;

            return ret;
        }
    }
}
