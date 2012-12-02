using Shooter.EntityInitialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class SpawnEntityComponent : IComponent
    {
        public IEntityFactory Factory;

        public IComponent Clone()
        {
            SpawnEntityComponent ret = new SpawnEntityComponent();
            ret.Factory = Factory.Clone();

            return ret;
        }
    }
}
