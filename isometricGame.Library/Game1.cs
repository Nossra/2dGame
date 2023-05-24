using isometricGame.Library.Models;
using isometricGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended; 
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic; 
using System.Linq;
using TiledSharp;
using static System.Net.Mime.MediaTypeNames;

namespace isometricGame.Library
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<Models.Component> _components;
        public static int ScreenHeight;
        public static int ScreenWidth;
        Sprite _player;
        Texture2D _coinTexture;
        Texture2D _tileSheet;
        TilemapManager _tilemapManager;
        SpriteFont _font;
        SpriteFont _signFont; 
        List<Map> _maps;
        const string HOUSE_LEFT_NAME = "LeftHouse";
        const string HOUSE_RIGHT_NAME = "RightHouse";
        const string TOWN_NAME = "Town";
        int tileWidth;
        int tileHeight;

        //player game state
        int playerCoins = 0; 

        //from extended, tiledmaps are used to get objectlayers for collisions and interactable objects
        TiledMap _townTiledMap;
        TiledMap _rightHouseTiledMap;
        TiledMap _leftHouseTiledMap;
        OrthographicCamera _camera;

        //from tiledsharp
        TmxMap _townTmxMap;
        TmxMap _rightHouseTmxMap;
        TmxMap _leftHouseTmxMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, ScreenWidth, ScreenHeight);
            _camera = new OrthographicCamera(viewportadapter);

            base.Initialize(); 
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadTextures();
            LoadTiledMaps();         //Used for object layers to get collisions and interactable objects
            LoadTmxMaps();           //the maps we have drawn in tiled
            LoadMaps();              //our custom map objects
            CreateTilemapManager();  //the manager responsible for drawing the map to the game
            
            _player = new Sprite(new Dictionary<string, Animation>()
            {
                { "WalkUp", new Animation(Content.Load<Texture2D>("Player/WalkingUp"), 3) },
                { "WalkDown", new Animation(Content.Load<Texture2D>("Player/WalkingDown"), 3) },
                { "WalkLeft", new Animation(Content.Load<Texture2D>("Player/WalkingLeft"), 3) },
                { "WalkRight", new Animation(Content.Load<Texture2D>("Player/WalkingRight"), 3) },
            })
            {
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                    Interact = Keys.E
                },
            };
            _player.Position = new Vector2(0,0);
             
            _components = new List<Models.Component>
            {
                _player,
            };

            var startMap = _maps.FirstOrDefault(x => x.Name == "Town");
            LoadMapData(startMap);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        { 
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {  
            foreach (var component in _components)
                component.Update(gameTime);
            CheckEntrance(); 
            CheckCollision();
            CollectCoin();
            InteractWithSign(); 
            _player.MoveSprite(); 
            _camera.LookAt(_player.Position);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); 
            _tilemapManager.Draw(); 
            spriteBatch.Begin();
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(_font, "Coins: " + playerCoins, new Vector2(10f, 10f), Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        } 

        private void LoadTextures()
        {
            _font = Content.Load<SpriteFont>("File");
            _signFont = Content.Load<SpriteFont>("SignText");
            _coinTexture = Content.Load<Texture2D>("Objects\\coin_transp");
            _tileSheet = Content.Load<Texture2D>("Map\\tilemap"); //the png file
        }

        private void LoadTiledMaps()
        {
            _townTiledMap = Content.Load<TiledMap>("Map\\townmap");
            _rightHouseTiledMap = Content.Load<TiledMap>("Map\\housemapright");
            _leftHouseTiledMap = Content.Load<TiledMap>("Map\\housemapleft");
        }

        private void LoadTmxMaps()
        {
            _townTmxMap = new TmxMap("Content\\Map\\townmap.tmx");
            _rightHouseTmxMap = new TmxMap("Content\\Map\\housemapright.tmx");
            _leftHouseTmxMap = new TmxMap("Content\\Map\\housemapleft.tmx");
        }

        private void LoadMaps()
        { 
            _maps = new List<Map>();
            var townEntrancesDictionary = new Dictionary<string, Entrance>()
            {
                { HOUSE_LEFT_NAME, new Entrance(GetEntranceRect(_townTiledMap,"leftHouseEntrance"),EntranceDirectionEnum.DOWN) },
                { HOUSE_RIGHT_NAME, new Entrance(GetEntranceRect(_townTiledMap,"rightHouseEntrance"), EntranceDirectionEnum.DOWN) }
            }; 

            var townMap = new Map(
                TOWN_NAME, 
                _townTmxMap, 
                true, 
                townEntrancesDictionary, 
                _townTiledMap.ObjectLayers.ToList(), 
                GraphicsDevice, 
                _coinTexture, 
                _signFont);
            _maps.Add(townMap);

            var leftHouseEnctranceDictionary = new Dictionary<string, Entrance>()
                {
                    { TOWN_NAME, new Entrance(GetEntranceRect(_leftHouseTiledMap,"townEntrance"), EntranceDirectionEnum.UP) }
                };

            _maps.Add(new Map(
                HOUSE_LEFT_NAME, 
                _leftHouseTmxMap, 
                false, 
                leftHouseEnctranceDictionary, 
                _leftHouseTiledMap.ObjectLayers.ToList(), 
                GraphicsDevice, 
                _coinTexture, 
                _signFont)); 

            var rightHouseEntranceDictionary = new Dictionary<string, Entrance>()
                {
                    { TOWN_NAME, new Entrance(GetEntranceRect(_rightHouseTiledMap,"townEntrance"), EntranceDirectionEnum.UP) }
                };

            _maps.Add(new Map(
                HOUSE_RIGHT_NAME, 
                _rightHouseTmxMap, 
                false, 
                rightHouseEntranceDictionary, 
                _rightHouseTiledMap.ObjectLayers.ToList(), 
                GraphicsDevice, 
                _coinTexture, 
                _signFont)); 
        }

        private void CreateTilemapManager()
        {
            tileWidth = _townTmxMap.Tilesets[0].TileWidth;
            tileHeight = _townTmxMap.Tilesets[0].TileHeight;
            int tilesetTilesWide = _tileSheet.Width / tileWidth;
            _tilemapManager = new TilemapManager(spriteBatch, _maps, _tileSheet, tilesetTilesWide, tileWidth, tileHeight);
        }
         
        private void CheckCollision()
        {
            Vector2 intendedMovement = _player.Position + _player.Movement; 
            int x = (int)intendedMovement.X;
            int y = (int)intendedMovement.Y;
            
            Rectangle intendedMovementRec = new Rectangle(x,y,_player.PlayerRectangle.Width, _player.PlayerRectangle.Height); 

            var activeMap = _maps.FirstOrDefault(map => map.Active);
            var collisionlayer = activeMap.ObjectLayers.FirstOrDefault(x => x.Name == "collisions");

            foreach (var obj in collisionlayer.Objects)
            {
                var collisionRect = GetRectangleFromTiledObject(obj);
                if (collisionRect.Intersects(intendedMovementRec))
                    _player.Movement = Vector2.Zero;
            }
        }
        
        private void CollectCoin()
        {
            var activeMap = _maps.FirstOrDefault(map => map.Active);

            foreach (var coin in activeMap.Coins)
            { 
                if (!coin.Collected)
                    if (coin.HitBox.Intersects(_player.PlayerRectangle)) {
                        coin.Collected = true;
                        playerCoins++;
                    }
            }
        } 

        private void InteractWithSign()
        {
            var activeMap = _maps.FirstOrDefault(map => map.Active);

            foreach (var signText in activeMap.SignTexts)
            {
                if (signText.InteractRectangle.Intersects(_player.PlayerRectangle) &&
                Keyboard.GetState().IsKeyDown(_player.Input.Interact) &&
                !signText.Opened)
                {
                    signText.Opened = true;
                }
                else if (!signText.InteractRectangle.Intersects(_player.PlayerRectangle))
                {
                    signText.Opened = false;
                }
            } 
        }
          
        private void CheckEntrance()
        {
            var activeMap = _maps.FirstOrDefault(x => x.Active == true);
            foreach (var entrance in activeMap.Entrances)
            {
                if (_player.PlayerRectangle.Intersects(entrance.Value.EntranceRectangle))
                { 
                    activeMap.Active = false;
                    var nextMap = _maps.FirstOrDefault(x => x.Name == entrance.Key); 

                    nextMap.Active = true;
                    SetPlayerPosition(nextMap, activeMap); 
                    LoadMapData(nextMap);
                }
            }
        } 

        private void SetPlayerPosition(Map nextMap, Map activeMap)
        {
            var entrance = nextMap.Entrances[activeMap.Name];

            var xOffset = 5;
            var yOffset = 0;
            if (entrance.Direction == EntranceDirectionEnum.UP)
            { 
                yOffset = - _player.PlayerRectangle.Height;
            } else
            {
                yOffset = 10;
            }

            _player.Position = new Vector2(
                entrance.EntranceRectangle.X + xOffset, 
                entrance.EntranceRectangle.Y + yOffset);
        } 
        private void LoadMapData(Map map)
        {
            _components = new()
            {
                _player
            };
            _components.AddRange(map.Coins);
            _components.AddRange(map.SignTexts);
        }
        
        private Rectangle GetRectangleFromTiledObject(TiledMapObject obj)
        {
            return new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height);
        }

        private Rectangle GetEntranceRect(TiledMap tileMap, string entranceObjectName)
        {
            var entrancesLayer = tileMap.GetLayer<TiledMapObjectLayer>("entrances");
            var entranceObject = entrancesLayer.Objects.FirstOrDefault(x => x.Name == entranceObjectName);
            return GetRectangleFromTiledObject(entranceObject);
        }
    }
}
