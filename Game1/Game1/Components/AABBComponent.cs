using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class AABBComponent : IComponent
    {
        public float Width;
        public float Height;

        public IComponent Clone()
        {
            AABBComponent ret = new AABBComponent();

            ret.Width = Width;
            ret.Height = Height;

            return ret;
        }
    }
}
