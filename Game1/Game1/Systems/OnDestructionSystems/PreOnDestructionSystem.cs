using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shooter.Systems.MarkForDestructionSystems;

namespace Shooter.Systems.OnDestructionSystems
{
    public class PreOnDestructionSystem : ASystem
    {
        public PreOnDestructionSystem()
        {
            SetParents(new List<Type>() { typeof(PostMarkForDestructionSystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
