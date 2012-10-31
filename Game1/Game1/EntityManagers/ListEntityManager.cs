using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.EntityManagers
{
    public class ListEntityManager : IEntityManager
    {
        public IList<Type> requiredComponents;
        public IList<Entity> entities;

        public ListEntityManager()
        {
            requiredComponents = new List<Type>();
            entities = new List<Entity>();
        }

        public void Add(Entity toAdd)
        {
            entities.Add(toAdd);
        }

        public void Remove(Entity toRemove)
        {
            entities.Remove(toRemove);
        }

        public IList<Type> GetRequiredComponents()
        {
            return requiredComponents;
        }
    }
}
