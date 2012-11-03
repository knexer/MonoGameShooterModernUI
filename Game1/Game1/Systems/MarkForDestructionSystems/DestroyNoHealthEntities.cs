using Microsoft.Xna.Framework;
using Shooter;
using Shooter.Components;
using Shooter.EntityManagers;
using Shooter.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.MarkForDestructionSystems
{
    public class DestroyNoHealthEntitiesSystem : ASystem
    {
        private ListEntityManager entities;

        public DestroyNoHealthEntitiesSystem()
        {
            SetParents(new List<Type>() { typeof(PreEntityMarkForDestructionSystem) });
            SetChildren(new List<Type>() { typeof(PostEntityMarkForDestructionSystem) });
            SetReqTypes(new List<Type>() { typeof(HealthComponent), typeof(DestroyedWhenNoHealthComponent) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            //find the dead entities and mark them for removal
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    HealthComponent hp = (HealthComponent)ent.components[typeof(HealthComponent)];
                    if (hp.Health <= 0)
                    {
                        ent.AddComponent(new MarkedForDeathComponent());
                    }
                }
            }
        }
    }
}
