using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    class EntityTranslationSystem : ASystem
    {
        private ListEntityManager entities;

        public EntityTranslationSystem()
        {
            SetReqTypes(new List<Type> { typeof(PositionDeltaComponent), typeof(PositionComponent) });
            SetParents(new List<Type> { typeof(PostCollisionResolutionDummySystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            //Get the components out of the entity
            PositionComponent pos = (PositionComponent)toProcess.components[typeof(PositionComponent)];
            PositionDeltaComponent delta = (PositionDeltaComponent)toProcess.components[typeof(PositionDeltaComponent)];

            //Update the entity's position
            pos.Position = pos.Position + delta.Delta;

            //This unit no longer needs to move
            toProcess.RemoveComponent(delta);
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in entities.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
