using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class LinearMovementSystem : ASystem
    {
        private ListEntityManager movers;

        public LinearMovementSystem()
        {
            SetReqTypes(new List<Type> { typeof(MoveSpeedComponent), typeof(LinearMovementComponent) });
            SetParents(new List<Type> { typeof(PreMovementStrategyDummySystem) });
            SetChildren(new List<Type> { typeof(PostMovementStrategyDummySystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            float speed = ((MoveSpeedComponent)toProcess.components[typeof(MoveSpeedComponent)]).MoveSpeed;
            float rotSpeed = ((LinearMovementComponent)toProcess.components[typeof(LinearMovementComponent)]).RotationRate;

            PositionDeltaComponent delta = new PositionDeltaComponent();
            delta.Delta = new Vector2(speed, 0);
            delta.RotationDelta = rotSpeed; 

            toProcess.AddComponent(delta);
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            movers = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in movers.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
