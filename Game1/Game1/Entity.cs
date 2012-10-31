using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    public class Entity
    {
        public static int NextId
        {
            get;
            private set;
        }

        static Entity()
        {
            NextId = 0;
        }

        public int Id
        {
            get;
            private set;
        }

        public Dictionary<Type, IComponent> components
        {
            get;
            private set;
        }

        public Entity()
        {
            Id = NextId;
            NextId++;

            components = new Dictionary<Type, IComponent>();
        }

        public void AddComponent(IComponent toAdd)
        {
            if (toAdd == null)
            {
                throw new ArgumentNullException();
            }

            if (components.ContainsKey(toAdd.GetType()))
            {
                throw new System.ArgumentException("Duplicate Component Added of Type " + toAdd.GetType() + "!");
            }

            components.Add(toAdd.GetType(), toAdd);
        }

        public void RemoveComponent(IComponent toRemove)
        {
            if (toRemove == null)
            {
                throw new ArgumentNullException();
            }

            RemoveComponent(toRemove.GetType());
        }

        private void RemoveComponent(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            components.Remove(type);
        }

        //Perform a (semi-)deep copy of this entity
        public Entity Clone()
        {
            Entity ret = new Entity();
            foreach (IComponent comp in components.Values)
            {
                ret.AddComponent(comp.Clone());
            }

            return ret;
        }
    }
}
