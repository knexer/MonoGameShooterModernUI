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
            throw new NotImplementedException();
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
