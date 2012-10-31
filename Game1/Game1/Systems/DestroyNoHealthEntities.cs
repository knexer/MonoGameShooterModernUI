using Game1.Components;
using Microsoft.Xna.Framework;
using Shooter;
using Shooter.EntityManagers;
using Shooter.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Systems
{
    public class DestroyNoHealthEntitiesSystem : ASystem
    {
        private ListEntityManager entities;

        public DestroyNoHealthEntitiesSystem()
        {
            SetParents(new List<Type>() { typeof(PositionSlavingSystem) });
            SetReqTypes(new List<Type>() { typeof(HealthComponent), typeof(DestroyedWhenNoHealthComponent) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            //find the dead entities
            List<Entity> marked = new List<Entity>();
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    HealthComponent hp = (HealthComponent)ent.components[typeof(HealthComponent)];
                    if (hp.Health <= 0)
                    {
                        marked.Add(ent);
                    }
                }
            }

            //remove the dead entities
            foreach (Entity toRemove in marked)
            {
                toProcess.Remove(toRemove);
            }
        }
    }
}
