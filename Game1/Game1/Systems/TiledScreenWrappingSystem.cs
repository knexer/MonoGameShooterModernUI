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
    /// <summary>
    /// Handles entities who are supposed to wrap around the edge of the screen (like in Asteroids).
    /// Add this component plus eight slave entities offset by every combo of screen width/height to do it
    /// </summary>
    public class TiledScreenWrappingSystem : ASystem
    {
        private ListEntityManager wrappers;

        private Viewport screen;

        public TiledScreenWrappingSystem(Viewport screen)
        {
            this.screen = screen;
            SetReqTypes(new List<Type> { typeof(PositionComponent), typeof(ScreenWrappingComponent), typeof(AABBComponent) });
            SetParents(new List<Type> { typeof(EntityTranslationSystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            //Fetch the components
            PositionComponent pos = (PositionComponent)toProcess.components[typeof(PositionComponent)];
            AABBComponent aabb = (AABBComponent)toProcess.components[typeof(AABBComponent)];

            //Compute whether the thing is entirely off the screen or not, and if so, wrap it
            //Goes off the right side case
            if (pos.Position.X > screen.Width)
            {
                //shift it one screen-width to the left
                pos.Position.X -= screen.Width;
            }

            //Goes off the left side case
            if (pos.Position.X < -aabb.Width)
            {
                pos.Position.X += screen.Width;
            }

            //Goes off the +y direction case
            if (pos.Position.Y > screen.Height)
            {
                pos.Position.Y -= screen.Height;
            }

            //Goes off the -y direction case
            if (pos.Position.Y < -aabb.Height)
            {
                pos.Position.Y += screen.Height;
            }
        }


        public override void Initialize(EntityManagerManager entityStorage)
        {
            wrappers = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in wrappers.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
