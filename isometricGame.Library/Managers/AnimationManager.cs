
using isometricGame.Library.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace isometricGame.Library.Managers
{
    public class AnimationManager
    {
        private Animation _animation;

        private float _timer;
        public Rectangle AnimationRectangle;
        public Vector2 Position;

        public AnimationManager(Animation animation)
        {
            _animation = animation; 
        } 

        public void Draw(SpriteBatch spriteBatch)
        {
            AnimationRectangle = new Rectangle(
            _animation.CurrentFrame * _animation.FrameWidth,
            0,
            _animation.FrameWidth,
            _animation.FrameHeight); 

            spriteBatch.Draw(_animation.Texture,
                             Position,
                             AnimationRectangle,
                             Color.White);
        }

        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
    }
}
