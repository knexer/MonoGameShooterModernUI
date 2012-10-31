using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    /// <summary>
    /// A dummy 'system' that exists only so that other systems can declare dependencies on each other through it instead of directly.
    /// </summary>
    public class PreMovementStrategyDummySystem : ASystem
    {
        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
