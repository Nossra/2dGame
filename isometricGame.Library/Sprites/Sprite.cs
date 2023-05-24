using isometricGame.Library.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using isometricGame.Library.Models;
using isometricGame.Library;

namespace isometricGame.Sprites
{
    public class Sprite : Component
    {
        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position;

        public float Speed = 2f; 
        public Vector2 Movement;
        public Input Input;
        public Rectangle PlayerRectangle; 
        public AnimationManager AnimationManager { get; set; }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (AnimationManager != null)
                    AnimationManager.Position = _position;
            }
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            AnimationManager = new AnimationManager(_animations.First().Value);
            PlayerRectangle = new Rectangle()
            {
                X = (int)Position.X,
                Y = (int)Position.Y,
                Height = _animations.First().Value.FrameHeight,
                Width = _animations.First().Value.FrameWidth
            };
        } 

        public virtual void SetMovement()
        {
            Movement = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Movement.Y = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Down))
                Movement.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Movement.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Movement.X = Speed; 
        }
         
        protected virtual void SetAnimations()
        {
            if (Movement.X > 0)
                AnimationManager.Play(_animations["WalkRight"]);
            else if (Movement.X < 0)
                AnimationManager.Play(_animations["WalkLeft"]);
            else if (Movement.Y > 0)
                AnimationManager.Play(_animations["WalkDown"]);
            else if (Movement.Y < 0)
                AnimationManager.Play(_animations["WalkUp"]);
            else AnimationManager.Stop();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationManager.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            SetMovement(); 
            SetAnimations();

            PlayerRectangle.X = (int)Position.X;
            PlayerRectangle.Y = (int)Position.Y;

            AnimationManager.Update(gameTime); 
        }

        public void MoveSprite()
        { 
            Position += Movement; 
        }
    }
}
