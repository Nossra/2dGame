using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace isometricGame.Library.Models
{
    public class Map
    {
        public string Name { get; set; }
        public TmxMap TmxMap { get; set; }
        public bool Active { get; set; }
        public Dictionary<string,Entrance> Entrances { get; set; }
        public List<TiledMapObjectLayer> ObjectLayers { get; set; }
        public List<Coin> Coins { get; set; }
        public List<SignText> SignTexts { get; set; }
        private GraphicsDevice graphics;
        private Texture2D _coinTexture;
        private SpriteFont _signFont;

        public Map(string name, TmxMap map, bool active, Dictionary<string, Entrance> entrances, List<TiledMapObjectLayer> objectLayers, GraphicsDevice graphics, Texture2D coinTexture, SpriteFont signFont)
        {
            Name = name;
            TmxMap = map;
            Active = active;
            Entrances = entrances;
            ObjectLayers = objectLayers; 
            this.graphics = graphics;
            _coinTexture = coinTexture;
            _signFont = signFont;

            LoadInteractables();
        }

        private void LoadInteractables()
        {
            if (Coins == null)
            {
                var coinLayer = ObjectLayers.FirstOrDefault(x => x.Name == "coins");
                Coins = LoadCoins(coinLayer);
            }
            if (SignTexts == null)
            {
                var signLayer = ObjectLayers.FirstOrDefault(x => x.Name == "signs");
                SignTexts = LoadSignTexts(signLayer);
            }
        }

        private List<Coin> LoadCoins(TiledMapObjectLayer coinLayer)
        {
            List<Coin> coins = new();
            if (coinLayer != null)
            {
                foreach (var obj in coinLayer.Objects)
                {
                    coins.Add(new Coin(_coinTexture, new Rectangle((int)obj.Position.X, (int)obj.Position.Y, 16, 16)));
                }

                return coins;
            }
            return coins;
        }

        private List<SignText> LoadSignTexts(TiledMapObjectLayer signLayer)
        {
            List<SignText> signTexts = new();
            if (signLayer != null)
            {
                var howToPlaySign = signLayer.Objects.FirstOrDefault(x => x.Name == "HowToPlay");
                if (howToPlaySign != null)
                {
                    var text = "Press E to interact. Guess you already knew that.";
                    int signInteractRadius = 20;
                    var signRect = new Rectangle(
                        (int)howToPlaySign.Position.X,
                        (int)howToPlaySign.Position.Y,
                        (int)howToPlaySign.Size.Width,
                        (int)(howToPlaySign.Size.Height + signInteractRadius));
                    signTexts.Add(new SignText(graphics, text, _signFont, signRect));
                }
                var houseSign = signLayer.Objects.FirstOrDefault(x => x.Name == "houseSign");
                if (houseSign != null)
                {
                    var text = "What are you doing in my house?";
                    int signInteractRadius = 20;
                    var signRect = new Rectangle(
                        (int)houseSign.Position.X,
                        (int)houseSign.Position.Y,
                        (int)houseSign.Size.Width,
                        (int)(houseSign.Size.Height + signInteractRadius));
                    signTexts.Add(new SignText(graphics, text, _signFont, signRect));
                }

                return signTexts;
            }
            return signTexts;
        }
    }
}
