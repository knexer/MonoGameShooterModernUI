using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class PeriodicAddComponentUpdater : ASystem
    {
        private ListEntityManager entities;

        public PeriodicAddComponentUpdater()
        {
            SetReqTypes(new List<Type> { typeof(PeriodicAddComponentComponent) });
            SetParents(new List<Type> { typeof(SpawnEntitySystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            PeriodicAddComponentComponent timer = (PeriodicAddComponentComponent)toProcess.components[typeof(PeriodicAddComponentComponent)];
            timer.TimeSinceLastFiring += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the timer hasn't been triggered in timer.Period many milliseconds
            if (timer.TimeSinceLastFiring >= timer.Period)
            {
                // reset it
                timer.TimeSinceLastFiring -= timer.Period;
                // and trigger it
                toProcess.AddComponent(timer.ComponentToAdd.Clone());
            }
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in entities.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
