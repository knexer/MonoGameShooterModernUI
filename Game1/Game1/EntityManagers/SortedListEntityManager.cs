using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.EntityManagers
{
    /// <summary>
    /// Manages a list of entities, keeps them sorted by stuff - and things also.
    /// </summary>
    public class SortedListEntityManager : IEntityManager
    {
        public IComparer<Entity> comparator;
        public IList<Type> requiredComponents;
        public IList<Entity> entities;

        public SortedListEntityManager()
        {
            requiredComponents = new List<Type>();
            entities = new List<Entity>();
        }

        public void Add(Entity toAdd)
        {
            entities.Add(toAdd);
            ((List<Entity>)entities).Sort(comparator);
        }

        public void Remove(Entity toRemove)
        {
            entities.Remove(toRemove);
            ((List<Entity>)entities).Sort(comparator);
        }

        // TODO: Keep the list sorted if the components are changed. (Which they most certainly will be, and often)

        public IList<Type> GetRequiredComponents()
        {
            return requiredComponents;
        }
    }
}
