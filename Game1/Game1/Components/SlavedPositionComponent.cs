using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    /// <summary>
    /// NOTE that this component isn't deep-copied correctly.
    /// Can anything be done about this???
    /// </summary>
    public class SlavedPositionComponent : IComponent
    {
        public Entity master;
        public Vector2 offset;

        public IComponent Clone()
        {
            throw new NotImplementedException();
        }
    }
}
