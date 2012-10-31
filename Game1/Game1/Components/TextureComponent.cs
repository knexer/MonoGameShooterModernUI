using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter.Components
{
    public class TextureComponent : IComponent
    {
        public Texture2D Texture;
        public Rectangle SourceRect;

        public IComponent Clone()
        {
            TextureComponent ret = new TextureComponent();
            ret.SourceRect = SourceRect;
            ret.Texture = Texture;

            return ret;
        }
    }
}
