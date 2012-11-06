using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Systems.MarkForDestructionSystems
{
    public class DestroyOffScreenEntitiesSystem : ASystem
    {
        private ListEntityManager entities;

        private Viewport screen;

        public DestroyOffScreenEntitiesSystem(Viewport screen)
        {
            SetReqTypes(new List<Type>() { typeof(DestroyedWhenOffScreenComponent), typeof(PositionComponent), typeof(AABBComponent) });
            SetParents(new List<Type>() { typeof(PreMarkForDestructionSystem) });
            SetChildren(new List<Type>() { typeof(PostMarkForDestructionSystem) });

            this.screen = screen;
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    if(isOffScreen(ent))
                    {
                        if (!ent.components.ContainsKey(typeof(MarkedForDeathComponent)))
                        {
                            ent.AddComponent(new MarkedForDeathComponent());
                        }
                    }
                }
            }
        }

        private bool isOffScreen(Entity ent)
        {
            PositionComponent pos = (PositionComponent)ent.components[typeof(PositionComponent)];
            AABBComponent aabb = (AABBComponent)ent.components[typeof(AABBComponent)];

            //If it's off the left side
            if (pos.Position.X + aabb.Width < 0)
                return true;

            //If it's off the top
            if (pos.Position.Y + aabb.Height < 0)
                return true;

            //If it's off the right
            if (pos.Position.X > screen.Width)
                return true;

            //Or if it's off the bottom
            if (pos.Position.Y > screen.Height)
                return true;

            return false;
        }
    }
}
