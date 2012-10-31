using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.EntityManagers
{
    public interface IEntityManager
    {
        IList<Type> GetRequiredComponents();
        void Add(Entity toAdd);
        void Remove(Entity toRemove);
    }
}
