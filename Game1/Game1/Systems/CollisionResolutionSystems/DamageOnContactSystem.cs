using Microsoft.Xna.Framework;
using Shooter;
using Shooter.Components;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Systems.CollisionResolutionSystems
{
    public class DamageOnContactSystem : ACollisionResolutionSystem
    {
        public ListEntityManager entities;

        public DamageOnContactSystem()
        {
            SetReqATypes(new List<Type>() { typeof(HealthComponent) });
            SetReqBTypes(new List<Type>() { typeof(DamageOnContactComponent) });
        }

        public override void Initialize(EntityManagerManager entityStorage)
        {
            entities = entityStorage.DefaultManager;
        }

        public override void Process(EntityManagerManager toProcess, GameTime gameTime)
        {
            foreach (Entity ent in entities.entities)
            {
                if (IsApplicableTo(ent))
                {
                    process(ent);
                }
            }
        }

        private void process(Entity toProcess)
        {
            CollisionComponent collision = (CollisionComponent)toProcess.components[typeof(CollisionComponent)];
            Entity damagee = collision.first;
            Entity damager = collision.second;

            DamageOnContactComponent damage = (DamageOnContactComponent)damager.components[typeof(DamageOnContactComponent)];
            HealthComponent hp = (HealthComponent)damagee.components[typeof(HealthComponent)];

            hp.Health -= damage.Damage;
        }
    }
}
