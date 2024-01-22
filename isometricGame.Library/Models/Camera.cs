using isometricGame.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isometricGame.Library.Models
{
    public class Camera
    {

        public Matrix Transform { get; private set; }
        public void Follow(Sprite target)
        {
            var position = Matrix.CreateTranslation(
              -target.Position.X,
              -target.Position.Y,
              0);
            var offset = Matrix.CreateTranslation( //dela med 2 på width och height om skalan i canvas inte är hårdkodad.
                Game1.ScreenWidth / 2,
                Game1.ScreenHeight / 2,
                0);

            Transform = position * offset;
        }
    }
} 
