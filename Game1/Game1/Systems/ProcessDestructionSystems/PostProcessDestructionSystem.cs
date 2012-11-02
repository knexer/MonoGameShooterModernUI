using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.ProcessDestructionSystems
{
    public class PostProcessDestructionSystem : ASystem
    {
        public PostProcessDestructionSystem()
        {
            SetParents(new List<Type>() { typeof(PreProcessDestructionSystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
