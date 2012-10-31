using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class RandomPositionOffsetSystem : ASystem
    {
        private ListEntityManager entities;

        public RandomPositionOffsetSystem()
        {
            SetReqTypes(new List<Type> { typeof(RandomPositionOffsetComponent) });
            SetParents(new List<Type> { typeof(PreMovementStrategyDummySystem) });
            SetChildren(new List<Type> { typeof(PostMovementStrategyDummySystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            // TODO pass this in so it can be controlled
            Random rand = new Random();

            RandomPositionOffsetComponent offset = (RandomPositionOffsetComponent)toProcess.components[typeof(RandomPositionOffsetComponent)];
            int minX = (int)Math.Min(offset.Maximum.X, offset.Minimum.X);
            int maxX = (int)Math.Max(offset.Maximum.X, offset.Minimum.X);

            int minY = (int)Math.Min(offset.Maximum.Y, offset.Minimum.Y);
            int maxY = (int)Math.Max(offset.Maximum.Y, offset.Minimum.Y);

            int x = rand.Next(minX, maxX);
            int y = rand.Next(minY, maxY);

            PositionDeltaComponent delta = new PositionDeltaComponent();
            if (toProcess.components.ContainsKey(typeof(PositionDeltaComponent)))
            {
                delta = (PositionDeltaComponent)toProcess.components[typeof(PositionDeltaComponent)];
                delta.Delta += new Vector2(x, y);
            }
            else
            {
                delta.Delta = new Vector2(x, y);
                toProcess.AddComponent(delta);
            }

            toProcess.RemoveComponent(offset);
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
