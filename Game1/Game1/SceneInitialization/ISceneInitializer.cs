using Microsoft.Xna.Framework;
using Shooter.EntityManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooter.SceneInitialization
{
    public interface ISceneInitializer
    {
        /// <summary>
        /// Adds this ISceneInitializer's Entities to the provided scene
        /// </summary>
        /// <param name="entityStore"></param>
        void InitializeScene(IEntityManager entityStore, Game game);
    }
}
