﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Systems.MarkForDestructionSystems
{
    public class PostMarkForDestructionSystem : ASystem
    {
        public PostMarkForDestructionSystem()
        {
            SetParents(new List<Type>() { typeof(PreMarkForDestructionSystem) });
        }

        public override void Initialize(EntityManagers.EntityManagerManager entityStorage)
        {
        }

        public override void Process(EntityManagers.EntityManagerManager toProcess, Microsoft.Xna.Framework.GameTime gameTime)
        {
        }
    }
}
