using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class ClampingSystem : ASystem
    {
        private ListEntityManager clamped;

        private Viewport screen;

        public ClampingSystem(Viewport screen)
        {
            this.screen = screen;
            SetReqTypes(new List<Type> { typeof(PositionComponent), typeof(AABBComponent), typeof(ScreenClampedComponent) });
            SetParents(new List<Type> { typeof(EntityTranslationSystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            // Make sure that the player does not go out of bounds
            PositionComponent pos = (PositionComponent)toProcess.components[typeof(PositionComponent)];
            AABBComponent box = (AABBComponent)toProcess.components[typeof(AABBComponent)];

            pos.Position.X = MathHelper.Clamp(pos.Position.X, 0, screen.Width - box.Width);
            pos.Position.Y = MathHelper.Clamp(pos.Position.Y, 0, screen.Height - box.Height);
        }


        public override void Initialize(EntityManagerManager entityStorage)
        {
            clamped = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in clamped.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
