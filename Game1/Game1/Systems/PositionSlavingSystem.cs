using Microsoft.Xna.Framework;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public class PositionSlavingSystem : ASystem
    {
        ListEntityManager slaves;

        public PositionSlavingSystem()
        {
            SetReqTypes(new List<Type> { typeof(PositionComponent) , typeof(SlavedPositionComponent) });
            SetParents(new List<Type> { typeof(EntityTranslationSystem), typeof(ClampingSystem), typeof(TiledScreenWrappingSystem) });
        }

        public void Process(Entity toProcess, GameTime gameTime)
        {
            if (!IsApplicableTo(toProcess))
            {
                return;
            }

            //Fetch the components
            PositionComponent pos = (PositionComponent)toProcess.components[typeof(PositionComponent)];
            SlavedPositionComponent slave = (SlavedPositionComponent)toProcess.components[typeof(SlavedPositionComponent)];

            //Fetch the position of the master entity
            PositionComponent masterPos = (PositionComponent)slave.master.components[typeof(PositionComponent)];

            //Update this slave's position to match
            pos.Position = masterPos.Position + slave.offset;
        }


        public override void Initialize(EntityManagerManager entityStorage)
        {
            slaves = entityStorage.DefaultManager;
        }


        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in slaves.entities)
            {
                Process(ent, gameTime);
            }
        }
    }
}
