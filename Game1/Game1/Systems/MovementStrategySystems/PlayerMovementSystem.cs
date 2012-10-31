using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class PlayerMovementSystem : ASystem
    {
        private ListEntityManager movers;

        public PlayerMovementSystem()
        {
            SetReqTypes(new List<Type> { typeof(MoveSpeedComponent), typeof(PlayerMovementComponent) });
            SetParents(new List<Type> { typeof(PreMovementStrategyDummySystem) });
            SetChildren(new List<Type> { typeof(PostMovementStrategyDummySystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            KeyboardState currentKeyboardState = Keyboard.GetState();

            Vector2 pos = new Vector2();

            float speed = ((MoveSpeedComponent)toProcess.components[typeof(MoveSpeedComponent)]).MoveSpeed;

            // Use the Keyboard
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                pos.X -= speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                pos.X += speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                pos.Y -= speed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                pos.Y += speed;
            }

            // Indicate that the player entity should be translated
            PositionDeltaComponent delta = new PositionDeltaComponent();
            delta.Delta = pos;
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
