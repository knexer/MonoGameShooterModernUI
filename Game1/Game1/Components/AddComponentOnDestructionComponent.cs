using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class AddComponentOnDestructionComponent : IComponent
    {
        public IComponent ToAdd;

        public IComponent Clone()
        {
            AddComponentOnDestructionComponent ret = new AddComponentOnDestructionComponent();
            ret.ToAdd = ToAdd;

            return ret;
        }
    }
}
