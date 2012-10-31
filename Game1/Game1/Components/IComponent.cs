using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public interface IComponent
    {
        IComponent Clone();
    }
}
