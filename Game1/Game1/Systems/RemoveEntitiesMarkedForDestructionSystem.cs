using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using Shooter.Systems.MarkForDestructionSystems;
using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems
{
    public class RemoveEntitiesMarkedForDestructionSystem : ASystem
    {
        private ListEntityManager entities;

        public RemoveEntitiesMarkedForDestructionSystem()
        {
            SetParents(new List<Type>() { typeof(PostEntityMarkForDestructionSystem) });
            SetReqTypes(new List<Type>() { typeof(MarkedForDeathComponent) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            List<Entity> toRemove = new List<Entity>();
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    toRemove.Add(ent);
                }
            }

            foreach (Entity ent in toRemove)
            {
                toProcess.Remove(ent);
            }
        }
    }
}
