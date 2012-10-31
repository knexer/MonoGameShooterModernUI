using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class BroadPhaseCollisionDetection : ASystem
    {
        private ListEntityManager collidables;

        private List<Entity> detectedCollisions;

        public BroadPhaseCollisionDetection()
        {
            SetReqTypes(new List<Type> { typeof(PositionComponent), typeof(AABBComponent) });
            SetParents(new List<Type> { typeof(PostMovementStrategyDummySystem) });
            SetChildren(new List<Type> { typeof(PreCollisionResolutionDummySystem) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            // TODO create a quadtree, x/y-sorted lists, etc. here
            collidables = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            detectedCollisions = new List<Entity>();

            //check every pair for AABB collisions
            foreach (Entity ent in collidables.entities)
            {
                if (!IsApplicableTo(ent))
                {
                    continue;
                }
                foreach (Entity ent2 in collidables.entities)
                {
                    if (IsApplicableTo(ent2))
                    {
                        process(ent, ent2, gameTime);
                    }
                }
            }

            //Add collision entities for each of those collisions
            foreach (Entity detected in detectedCollisions)
            {
                toProcess.Add(detected);
            }
        }

        private void process(Entity ent, Entity ent2, GameTime gameTime)
        {
            PositionComponent pos1 = (PositionComponent)ent.components[typeof(PositionComponent)];
            AABBComponent aabb1 = (AABBComponent)ent.components[typeof(AABBComponent)];

            PositionComponent pos2 = (PositionComponent)ent2.components[typeof(PositionComponent)];
            AABBComponent aabb2 = (AABBComponent)ent2.components[typeof(AABBComponent)];

            if (isPotentialCollision(pos1, aabb1, pos2, aabb2))
            {
                //Create an entity with a collision component
                Entity collisionEntity = new Entity();
                CollisionComponent collision = new CollisionComponent();
                collision.first = ent;
                collision.second = ent2;
                collisionEntity.AddComponent(collision);

                //Add it to the list of potential collisions
                detectedCollisions.Add(collisionEntity);
            }
        }

        private bool isPotentialCollision(PositionComponent pos1, AABBComponent aabb1, PositionComponent pos2, AABBComponent aabb2)
        {
            //if 1 is entirely right of 2
            if (pos1.Position.X > pos2.Position.X + aabb2.Width)
                return false;

            //if 1 is entirely below 2
            if (pos1.Position.Y > pos2.Position.Y + aabb2.Height)
                return false;

            //if 2 is entirely right of 1
            if (pos2.Position.X > pos1.Position.X + aabb1.Width)
                return false;

            //if 2 is entirely below 1
            if (pos2.Position.Y > pos1.Position.Y + aabb1.Height)
                return false;

            //gauntlet run complete!
            return true;
        }
    }
}
