using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class ScreenClampedComponent : IComponent
    {
        public IComponent Clone()
        {
            return new ScreenClampedComponent();
        }
    }
}
