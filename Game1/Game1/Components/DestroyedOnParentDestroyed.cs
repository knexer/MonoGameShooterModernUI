using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class DestroyedOnParentDestroyedComponent : IComponent
    {
        public Entity parent;

        public IComponent Clone()
        {
            DestroyedOnParentDestroyedComponent ret = new DestroyedOnParentDestroyedComponent();
            ret.parent = parent;

            return ret;
        }
    }
}
