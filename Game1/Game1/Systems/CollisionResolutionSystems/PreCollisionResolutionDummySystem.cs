using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class PreCollisionResolutionDummySystem : ASystem
    {
        public PreCollisionResolutionDummySystem()
        {
            SetChildren(new List<Type>() { typeof(PostCollisionResolutionDummySystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
