using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.EntityManagers
{
    public class EntityManagerManager : IEntityManager
    {
        public ListEntityManager DefaultManager
        {
            get;
            private set;
        }

        private List<IEntityManager> entityManagers;

        public EntityManagerManager()
        {
            DefaultManager = new ListEntityManager();
            entityManagers = new List<IEntityManager>();
            entityManagers.Add(DefaultManager);
        }

        public void AddEntityManager(IEntityManager toAdd)
        {
            entityManagers.Add(toAdd);
        }

        //Add the entity to every entity manager it can go in
        public void Add(Entity toAdd)
        {
            //Add the new entity to every entity manager that it has the components to support
            foreach (IEntityManager manager in entityManagers)
            {
                IEnumerable<Type> reqTypes = manager.GetRequiredComponents();

                //compute whether the entity to be added has a component of every type necessary for management by each of the entity managers
                bool shouldAdd = true;
                //foreach type required by the current manager
                foreach (Type reqType in reqTypes)
                {
                    //if toAdd doesn't contain a component of that type
                    if (!toAdd.components.ContainsKey(reqType))
                    {
                        //mark this as not valid for management
                        shouldAdd = false;
                        break;
                    }
                }

                // If the entity is still valid to be managed by the current manager
                if (shouldAdd)
                {
                    // Add it to the current manager
                    manager.Add(toAdd);
                }
            }
        }

        //Removes the entity from every entity manager
        public void Remove(Entity toRemove)
        {
            //Remove the entity from every entity manager that it is in (has the components to support)
            foreach (IEntityManager manager in entityManagers)
            {
                IEnumerable<Type> reqTypes = manager.GetRequiredComponents();

                bool shouldRemove = true;
                foreach (Type reqType in reqTypes)
                {
                    if (!toRemove.components.ContainsKey(reqType))
                    {
                        shouldRemove = false;
                        break;
                    }
                }
                if (shouldRemove)
                {
                    manager.Remove(toRemove);
                }
            }
        }

        //Add the entity to the entity managers it would be able to go in after the new component is added
        public void OnComponentAdded(Entity toAdd, Type compType)
        {
            foreach (IEntityManager manager in entityManagers)
            {
                IEnumerable<Type> reqTypes = manager.GetRequiredComponents();

                bool shouldAdd = true;
                foreach (Type reqType in reqTypes)
                {
                    if (!toAdd.components.ContainsKey(reqType))
                    {
                        shouldAdd = false;
                        break;
                    }
                }
                if (shouldAdd)
                {
                    if (reqTypes.Contains(compType))
                    {
                        manager.Add(toAdd);
                    }
                }
            }
        }

        public void OnComponentRemoved(Entity toRemove, Type compType)
        {
            foreach (IEntityManager manager in entityManagers)
            {
                IEnumerable<Type> reqTypes = manager.GetRequiredComponents();

                if (reqTypes.Contains(compType))
                {
                    manager.Remove(toRemove);
                }
            }
        }

        public IList<Type> GetRequiredComponents()
        {
            throw new NotImplementedException();
        }
    }
}
