using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class AnimationUpdateSystem : ASystem
    {
        private ListEntityManager animated;

        public AnimationUpdateSystem()
        {
            SetReqTypes(new List<Type> { typeof(AnimationComponent) });
            SetParents(new List<Type> { typeof(PeriodicAddComponentUpdater) });
            SetChildren(new List<Type>());
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            //Fetch the components
            AnimationComponent animation = (AnimationComponent)toProcess.components[typeof(AnimationComponent)];

            //elapse time for the animation
            animation.TimeSinceFrameChange += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            //handle a frame-change event
            if (animation.TimeSinceFrameChange >= animation.FrameDuration)
            {
                //fix the elapsed tiem
                animation.TimeSinceFrameChange -= animation.FrameDuration;

                //increment to the new frame
                animation.CurrentFrameIndex++;

                //handle looping or de-activating
                if (animation.CurrentFrameIndex >= animation.NumFrames)
                {
                    if (animation.Looping)
                    {
                        animation.CurrentFrameIndex = 0;
                    }
                    else
                    {
                        animation.CurrentFrameIndex = -1;
                    }
                }
            }
        }


        public override void Initialize(EntityManagerManager entityStorage)
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
