using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class LinearMovementComponent : IComponent
    {
        public IComponent Clone()
        {
            return new LinearMovementComponent();
        }
    }
}
