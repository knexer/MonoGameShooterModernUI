using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Systems
{
    public abstract class ASystem
    {
        //The types of components that must be present for this system to operate
        private IEnumerable<Type> reqTypes;

        //The types of systems that this system must run after
        private IEnumerable<Type> parents;

        //The types of systems that this system must run before
        private IEnumerable<Type> children;

        public ASystem()
        {
            reqTypes = new List<Type>();
            parents = new List<Type>();
            children = new List<Type>();
        }

        protected void SetReqTypes(List<Type> reqTypes)
        {
            this.reqTypes = reqTypes;
        }

        protected void SetParents(List<Type> parents)
        {
            this.parents = parents;
        }

        protected void SetChildren(List<Type> children)
        {
            this.children = children;
        }

        public IEnumerable<Type> GetRequiredComponentTypes()
        {
            return reqTypes;
        }

        public IEnumerable<Type> GetParents()
        {
            return parents;
        }

        public IEnumerable<Type> GetChildren()
        {
            return children;
        }

        public abstract void Initialize(EntityManagerManager entityStorage);

        public abstract void Process(EntityManagerManager toProcess, GameTime gameTime);

        public bool IsApplicableTo(Entity candidate)
        {
            //Ensure that the Entity should truly be processed
            foreach (Type t in GetRequiredComponentTypes())
            {
                if (!candidate.components.ContainsKey(t))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
