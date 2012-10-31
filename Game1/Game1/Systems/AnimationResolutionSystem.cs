using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    /// <summary>
    /// Resolves animations into actual texture source rectangles (picks a sprite from a sprite strip based on animation state)
    /// </summary>
    public class AnimationResolutionSystem : ASystem
    {
        private ListEntityManager animated;

        public AnimationResolutionSystem()
        {
            SetReqTypes(new List<Type> { typeof(AnimationComponent), typeof(TextureComponent) });
            SetChildren(new List<Type> { typeof(RenderSystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            //Fetch the components
            AnimationComponent animation = (AnimationComponent)toProcess.components[typeof(AnimationComponent)];
            TextureComponent tex = (TextureComponent)toProcess.components[typeof(TextureComponent)];

            //Compute the size of a texture frame
            int frameWidth = tex.Texture.Width / animation.NumFrames;
            int frameHeight = tex.Texture.Height;

            //Compute the x-offset of the current texture frame
            int frameX = frameWidth * animation.CurrentFrameIndex;

            //Compute and store the source rectangle for this frame
            tex.SourceRect = new Rectangle(frameX, 0, frameWidth, frameHeight);
        }


        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
            animated = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in animated.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
