using isometricGame.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using isometricGame.Models;

namespace isometricGame.Sprites
{
    public class Sprite
    {
        #region Fields

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position; 

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 2f;

        public Vector2 Vector;

        #endregion

        #region Methods

        public virtual void Draw(SpriteBatch spriteBatch)
        {  
            _animationManager.Draw(spriteBatch); 
        }

        public virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Vector.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Vector.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Vector.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Vector.X = Speed;

            if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Vector.Y = -Speed;
                Vector.X = -Speed;
            } else if (Keyboard.GetState().IsKeyDown(Input.Down) && Keyboard.GetState().IsKeyDown(Input.Left))
            {
                Vector.Y = Speed;
                Vector.X = -Speed;
            } else if (Keyboard.GetState().IsKeyDown(Input.Up) && Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Vector.Y = -Speed;
                Vector.X = Speed;
            } else if (Keyboard.GetState().IsKeyDown(Input.Down) && Keyboard.GetState().IsKeyDown(Input.Right))
            {
                Vector.Y = Speed;
                Vector.X = Speed;
            }



        }

        protected virtual void SetAnimations()
        {
            if (Vector.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Vector.X < 0)
                _animationManager.Play(_animations["WalkLeft"]);
            else if (Vector.Y > 0)
                _animationManager.Play(_animations["WalkDown"]);
            else if (Vector.Y < 0)
                _animationManager.Play(_animations["WalkUp"]);
            else _animationManager.Stop();
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        } 

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();

            SetAnimations();

            _animationManager.Update(gameTime);

            Position += Vector;
            Vector = Vector2.Zero;
        }

        #endregion
    }
}
