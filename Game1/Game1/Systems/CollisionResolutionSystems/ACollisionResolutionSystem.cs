using Shooter;
using Shooter.Components;
using Shooter.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Systems.CollisionResolutionSystems
{
    public abstract class ACollisionResolutionSystem : ASystem
    {
        //The types of components required on Entity A
        private IEnumerable<Type> reqATypes;

        //The types of components required on Entity B
        private IEnumerable<Type> reqBTypes;

        public ACollisionResolutionSystem()
        {
            reqATypes = new List<Type>();
            reqBTypes = new List<Type>();

            SetReqTypes(new List<Type>() { typeof(CollisionComponent) });
            SetParents(new List<Type>() { typeof(PreCollisionResolutionDummySystem) });
            SetChildren(new List<Type>() { typeof(PostCollisionResolutionDummySystem) });
        }

        public void SetReqATypes(List<Type> reqATypes)
        {
            this.reqATypes = reqATypes;
        }

        public void SetReqBTypes(List<Type> reqBTypes)
        {
            this.reqBTypes = reqBTypes;
        }

        public IEnumerable<Type> GetReqATypes()
        {
            return reqATypes;
        }

        public IEnumerable<Type> GetReqBTypes()
        {
            return reqBTypes;
        }

        public override bool IsApplicableTo(Entity other)
        {
            if (!other.components.ContainsKey(typeof(CollisionComponent)))
                return false;

            CollisionComponent collision = (CollisionComponent)other.components[typeof(CollisionComponent)];

            Entity a = collision.first;
            Entity b = collision.second;

            return isApplicableTo(a, b);
            
            //Uncomment this to make collision resolution systems not care about order (may or make not make sense)
            //return isApplicableTo(a, b) || isApplicableTo(b, a);
        }

        private bool isApplicableTo(Entity a, Entity b)
        {
            return isApplicableTo(a, reqATypes) && isApplicableTo(b, reqBTypes);
        }

        private bool isApplicableTo(Entity candidate, IEnumerable<Type> reqTypes)
        {
            //Ensure that the Entity should truly be processed
            foreach (Type t in reqTypes)
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
