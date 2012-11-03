﻿using Shooter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.Components
{
    public class DestroyedWhenNoHealthComponent : IComponent
    {
        public IComponent Clone()
        {
            return new DestroyedWhenNoHealthComponent();
        }
    }
}
