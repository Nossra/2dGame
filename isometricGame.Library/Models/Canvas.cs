using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isometricGame.Library.Models
{
    public class Canvas
    {
        private readonly RenderTarget2D _target;
        private readonly GraphicsDevice _graphicsDevice;
        private Rectangle _destinationRectangle;

        public Canvas(GraphicsDevice graphicsDevice, int width, int height)
        {
            _graphicsDevice = graphicsDevice;
            _target = new RenderTarget2D(graphicsDevice, width, height);
        }

        public void SetDestinationRectangle()
        {
            var screenSize = _graphicsDevice.PresentationParameters.Bounds;

            float scaleX = (float)screenSize.Width / _target.Width;
            float scaleY = (float)screenSize.Height / _target.Height;
            float scale = Math.Min(scaleX, scaleY);

            int newWidth = (int) (_target.Width *  scale);
            int newHeight = (int) (_target.Height * scale);

            int posX = (screenSize.Width - newWidth) / 2;
            int posY = (screenSize.Height - newHeight) / 2;

            _destinationRectangle = new Rectangle(posX,posY, newWidth, newHeight);
        }

        public void Activate()
        {
            _graphicsDevice.SetRenderTarget(_target);
            _graphicsDevice.Clear(Color.Black);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_target, _destinationRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
