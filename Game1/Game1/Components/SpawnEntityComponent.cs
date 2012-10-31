using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class SpawnEntityComponent : IComponent
    {
        public Entity EntityToSpawn;

        public IComponent Clone()
        {
            SpawnEntityComponent ret = new SpawnEntityComponent();
            ret.EntityToSpawn = EntityToSpawn.Clone();

            return ret;
        }
    }
}
