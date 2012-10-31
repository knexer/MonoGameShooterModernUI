using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class CollisionComponent : IComponent
    {
        public Entity first;
        public Entity second;

        public IComponent Clone()
        {
            CollisionComponent ret = new CollisionComponent();
            ret.first = first;
            ret.second = second;

            return ret;
        }
    }
}
