using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isometricGame.Library.Models
{
    public class SignText : Component
    {
        public string Text { get; set; }
        public Rectangle InteractRectangle { get; set; }
        public SpriteFont Font { get; set; }

        public Texture2D BackgroundTexture { get; set; }
        public bool Opened { get; set; }

        Vector2 backgroundVector;
        int backgroundPadding = 5;
        Rectangle backGroundRectangle;

        public SignText(GraphicsDevice gfx, string text, SpriteFont font, Rectangle interactRectangle) 
        {
            Text = text;
            InteractRectangle = interactRectangle;
            Font = font;

            BackgroundTexture = new Texture2D(gfx, 1, 1);
            BackgroundTexture.SetData(new[] { Color.Black });
            backgroundVector = Font.MeasureString(Text); 
            backGroundRectangle = new Rectangle(
                InteractRectangle.X - backgroundPadding,
                InteractRectangle.Y - backgroundPadding,
                (int)backgroundVector.X + backgroundPadding * 2,
                (int)backgroundVector.Y + backgroundPadding * 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Opened)
            { 
                spriteBatch.Draw(BackgroundTexture, backGroundRectangle, Color.Black);
                spriteBatch.DrawString(Font, Text, new Vector2(InteractRectangle.X, InteractRectangle.Y), Color.White);
            }

        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
