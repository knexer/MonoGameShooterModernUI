using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.OnDestructionSystems
{
    public class PostOnDestructionSystem : ASystem
    {
        public PostOnDestructionSystem()
        {
            SetParents(new List<Type>() { typeof(PreOnDestructionSystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
