using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class SpawnEntityAtPositionComponent : IComponent
    {
        public Entity toSpawn;

        public IComponent Clone()
        {
            SpawnEntityAtPositionComponent spawner = new SpawnEntityAtPositionComponent();
            spawner.toSpawn = toSpawn.Clone();

            return spawner;
        }
    }
}
