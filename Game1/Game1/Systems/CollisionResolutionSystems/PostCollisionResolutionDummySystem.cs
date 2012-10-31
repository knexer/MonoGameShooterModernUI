using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class PostCollisionResolutionDummySystem : ASystem
    {
        private ListEntityManager collisions;

        public PostCollisionResolutionDummySystem()
        {
            SetReqTypes(new List<Type> { typeof(CollisionComponent) });
            SetParents(new List<Type> { typeof(PreCollisionResolutionDummySystem) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            collisions = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            List<Entity> marked = new List<Entity>();
            foreach (Entity toDiscard in collisions.entities)
            {
                if (IsApplicableTo(toDiscard))
                {
                    marked.Add(toDiscard);
                }
            }

            foreach (Entity toRemove in marked)
            {
                toProcess.Remove(toRemove);
            }
        }
    }
}
