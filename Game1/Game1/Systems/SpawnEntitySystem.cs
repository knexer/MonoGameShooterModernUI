using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using Shooter.Systems.OnDestructionSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class SpawnEntitySystem : ASystem
    {

        public SpawnEntitySystem()
        {
            SetReqTypes(new List<Type> { typeof(SpawnEntityComponent) });
            SetParents(new List<Type>() { typeof(PostOnDestructionSystem) });
            SetChildren(new List<Type>() { typeof(RemoveEntitiesMarkedForDestructionSystem) });
        }

        private ListEntityManager entities;

        public Entity Process(EntityManagerManager manager, Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return null;
            }

            //Get the component
            SpawnEntityComponent spawner = (SpawnEntityComponent)toProcess.components[typeof(SpawnEntityComponent)];

            //Construct a copy of the entity to be spawned
            Entity toSpawn = spawner.EntityToSpawn.Clone();

            //Remove the offending component so we don't get more spawns
            toProcess.RemoveComponent(spawner);

            //Spawn that entity by adding it to the list of entities to be updated by systems
            return toSpawn;
        }
    
        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            //Get the list of entities that need to be spawned this tick
            IList<Entity> toSpawn = new List<Entity>();
            foreach (Entity ent in entities.entities)
            {
                Entity tentative = Process(toProcess, ent, gameTime);
                if (tentative != null)
                {
                    toSpawn.Add(tentative);
                }
            }

            // Spawn them all by adding them to the EntityManagers
            foreach (Entity toBeManaged in toSpawn)
            {
                toProcess.Add(toBeManaged);
            }
        }
    }
}
