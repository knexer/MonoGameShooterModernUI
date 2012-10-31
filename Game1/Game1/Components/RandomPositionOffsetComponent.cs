using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    /// <summary>
    /// Generates a position offset drawn from the specified uniform distribution.
    /// </summary>
    public class RandomPositionOffsetComponent : IComponent
    {
        public Vector2 Minimum, Maximum;

        public RandomPositionOffsetComponent()
        {
            Minimum = new Vector2();
            Maximum = new Vector2();
        }

        public IComponent Clone()
        {
            RandomPositionOffsetComponent ret = new RandomPositionOffsetComponent();
            ret.Minimum = Minimum;
            ret.Maximum = Maximum;

            return ret;
        }
    }
}
