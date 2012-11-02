using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.MarkForDestructionSystems
{
    public class PostEntityMarkForDestructionSystems : ASystem
    {
        public PostEntityMarkForDestructionSystems()
        {
            SetParents(new List<Type>() { typeof(PreEntityMarkForDestructionSystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
