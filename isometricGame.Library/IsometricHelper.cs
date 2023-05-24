using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace isometricGame.Library
{
    public class IsometricHelper
    {
        private int tileWidth;
        private int tileHeight;

        public IsometricHelper(int tileWidth, int tileHeight)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
        public Point ScreenToMap(int x, int y)
        {
            var tileWidthHalf = tileWidth / 2;
            var tileHeightHalf = tileHeight / 2;

            var mapX = (x / tileWidthHalf + y / tileHeightHalf) / 2;
            var mapY = (y / tileHeightHalf - (x / tileWidthHalf)) / 2;

            return new(mapX, mapY);
        }

        public Point MapToScreen(int x, int y) 
        { 
            int screenX = (x - y) * (tileWidth / 2);
            int screenY = (x + y) * (tileHeight / 4); 

            return new(screenX, screenY);
        }

        public Point CartographicCoordinates(int x, int y)
        {
            return new(x*tileWidth, y*tileHeight);
        }
    }
}
