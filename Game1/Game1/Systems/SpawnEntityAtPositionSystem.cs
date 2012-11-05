using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using Shooter.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shooter.Systems.OnDestructionSystems;

namespace Shooter.Systems
{
    public class SpawnEntityAtPositionSystem : ASystem
    {
        private ListEntityManager entities;

        public SpawnEntityAtPositionSystem()
        {
            SetReqTypes(new List<Type>() { typeof(PositionComponent), typeof(SpawnEntityAtPositionComponent) });
            SetParents(new List<Type>() { typeof(PostOnDestructionSystem) });
            SetChildren(new List<Type>() { typeof(RemoveEntitiesMarkedForDestructionSystem) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            //Build the list of entities to be spawned
            List<Entity> toSpawn = new List<Entity>();
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    //Create the entity to be spawned
                    SpawnEntityAtPositionComponent spawner = (SpawnEntityAtPositionComponent)ent.components[typeof(SpawnEntityAtPositionComponent)];
                    Entity toAdd = spawner.toSpawn.Clone();

                    //Set its position to this entity's position
                    PositionComponent pos = (PositionComponent)ent.components[typeof(PositionComponent)];
                    toAdd.AddComponent(pos.Clone());

                    toSpawn.Add(toAdd);

                    //Finally remove the spawner component so that we don't continuously spawn
                    ent.RemoveComponent(spawner);
                }
            }

            //Add the found entities
            foreach (Entity toAdd in toSpawn)
            {
                toProcess.Add(toAdd);
            }
        }
    }
}
