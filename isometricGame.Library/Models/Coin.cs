using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isometricGame.Library.Models
{
    public class Coin : Component
    {
        public Texture2D Texture { get; set; }
        public Rectangle HitBox { get; set; }
        public bool Collected { get; set; }

        public Coin(Texture2D texture, Rectangle hitBox)
        {
            Texture = texture;
            HitBox = hitBox;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Collected)
            {
                spriteBatch.Draw(Texture, HitBox, Color.White);
            } 
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
