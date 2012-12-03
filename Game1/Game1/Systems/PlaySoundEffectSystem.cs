using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using Shooter.Systems.OnDestructionSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems
{
    public class PlaySoundEffectSystem : ASystem
    {
        private ListEntityManager entities;

        public PlaySoundEffectSystem()
        {
            SetReqTypes(new List<Type>() { typeof(SoundEffectComponent) });
            SetChildren(new List<Type>() { typeof(SpawnEntitySystem)});
            SetParents(new List<Type>() { typeof(PostOnDestructionSystem) });
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
                    SoundEffectComponent comp = (SoundEffectComponent)ent.components[typeof(SoundEffectComponent)];
                    comp.effect.Play();

                    ent.RemoveComponent(comp);
                }
            }
        }
    }
}
