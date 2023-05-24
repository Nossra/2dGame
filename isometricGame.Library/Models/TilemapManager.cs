using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 
using System;
using System.Collections.Generic; 
using System.Linq; 
using TiledSharp;

namespace isometricGame.Library.Models
{
    public class TilemapManager
    {
        SpriteBatch spriteBatch;
        public List<Map> maps;
        Texture2D tileset;
        int tilesetTilesWide; 

        public int TileHeight { get; set; }
        public int TileWidth { get; set; } 

        public TilemapManager(SpriteBatch spriteBatch,List<Map> maps, Texture2D tileset, int tilesetTilesWide, int tileWidth, int tileHeight)
        {
            this.maps = maps;
            this.tileset = tileset;
            this.tilesetTilesWide = tilesetTilesWide;  
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            this.spriteBatch = spriteBatch;
        }

        private static T[,] Make2DArray<T>(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }
 
        public void Draw()
        {
            spriteBatch.Begin();
            if (maps.FindAll(x => x.Active == true).Count > 1) throw new Exception("Too many active maps.");

            var map = maps.FirstOrDefault(x => x.Active == true);
            foreach (var layer in map.TmxMap.Layers)
            {
                if (layer.Visible)
                {
                    var mapTiles = Make2DArray<TmxLayerTile>(layer.Tiles.ToArray(), 20, 30);

                    for (int y = 0; y < mapTiles.GetLength(0); y++)
                    {
                        for (int x = 0; x < mapTiles.GetLength(1); x++)
                        {
                            if (mapTiles[y, x].Gid != 0)
                            {
                                int tileFrame = mapTiles[y, x].Gid - 1;
                                int column = tileFrame % tilesetTilesWide;
                                int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                                Rectangle tilesetRec = new Rectangle(TileWidth * column, TileHeight * row, TileWidth, TileHeight);

                                var helper = new IsometricHelper(TileWidth, TileHeight);

                                var cartographic = helper.CartographicCoordinates(x, y);
                                Rectangle cartographicRectangle = new Rectangle(cartographic.X, cartographic.Y, TileWidth, TileHeight);
                                spriteBatch.Draw(tileset, cartographicRectangle, tilesetRec, Color.White);
                            }

                        }
                    }
                }

            }
            spriteBatch.End();
        }
    }
}
