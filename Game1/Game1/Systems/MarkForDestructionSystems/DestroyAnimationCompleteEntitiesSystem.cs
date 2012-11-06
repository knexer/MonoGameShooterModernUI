using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.MarkForDestructionSystems
{
    public class DestroyAnimationCompleteEntitiesSystem : ASystem
    {
        private ListEntityManager entities;

        public DestroyAnimationCompleteEntitiesSystem()
        {
            SetReqTypes(new List<Type>() { typeof(DestroyedWhenAnimationCompleteComponent), typeof(AnimationComponent) });
            SetParents(new List<Type>() { typeof(PreMarkForDestructionSystem) });
            SetChildren(new List<Type>() { typeof(PostMarkForDestructionSystem) });
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
                    AnimationComponent anim = (AnimationComponent)ent.components[typeof(AnimationComponent)];

                    //If the animation is complete
                    if (anim.CurrentFrameIndex == -1 && !ent.components.ContainsKey(typeof(MarkedForDeathComponent)))
                    {
                        ent.AddComponent(new MarkedForDeathComponent());
                    }
                }
            }
        }
    }
}
