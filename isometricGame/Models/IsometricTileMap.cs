using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TiledSharp;

namespace isometricGame.Desktop
{
    public class IsometricTileMap
    {
        private SpriteBatch spriteBatch;
        private TmxMap map;
        private int tilesetTilesWide;
        private int tileWidth;
        private int tileHeight;
        private Texture2D tileset;

        public IsometricTileMap(SpriteBatch spriteBatch, TmxMap map, int tilesetTilesWide, int tileWidth, int tileHeight, Texture2D tileset)
        {
            this.spriteBatch = spriteBatch;
            this.map = map;
            this.tilesetTilesWide = tilesetTilesWide;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileset = tileset;
        }

        private void Draw()
        {
            spriteBatch.Begin();
            foreach (var layer in map.Layers)
            {
                for (int i = 0; i < layer.Tiles.Count; i++)
                {
                    int gid = layer.Tiles[i].Gid;
                    if (gid != 0)
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double) tilesetTilesWide);
                        Rectangle tilesetRectangle = new Rectangle(tileWidth * column,
                                                                   tileHeight * row,
                                                                   tileWidth,
                                                                   tileHeight);
                        float x = (i % map.Width) * map.TileHeight / 2;
                        float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                        //cartesian coordinates to isometric coordinates
                        int isoX = (int)(x - y);
                        int isoY = (int)(x + y) / 2;

                        spriteBatch.Draw(tileset, new Rectangle(isoX, isoY, tileWidth, tileHeight), tilesetRectangle, Color.White);
                    }
                } 
            }
            spriteBatch.End();
        }
    }
}
