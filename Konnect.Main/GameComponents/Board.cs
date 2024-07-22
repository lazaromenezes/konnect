using Konnect.Framework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Konnect.Main.GameComponents
{
    internal class Board : DrawableGameComponent
    {
        const int Y_OFFSET = 40;
        const int X_OFFSET = 40;

        const int ROOM_COUNT = 10;
        const int DOT_COUNT = ROOM_COUNT + 1;

        readonly List<List<Dot>> _dots = [];
        readonly List<List<Room>> _rooms = [];

        readonly SpriteBatch _spriteBatch;

        Dot currentDot;

        Texture2D _dotSprite, _wallSprite, _tileSprite;

        public Board(Game game) : base(game)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            
            InitializeRooms();
            InitializeDots();
        }

        private void InitializeDots()
        {
            for (var row = 0; row < DOT_COUNT; row++)
            {
                _dots.Add([]);
                for (var column = 0; column < DOT_COUNT; column++)
                {
                    AddDotAt(row, column);
                }
            }
        }

        private void InitializeRooms()
        {
            for (var row = 0; row < ROOM_COUNT; row++)
            {
                _rooms.Add([]);
                for (var column = 0; column < ROOM_COUNT; column++)
                {
                    AddRoomAt(row, column);
                }
            }
        }

        private void AddDotAt(int row, int column)
        {
            var index = row * DOT_COUNT + column;

            var dot = new Dot(index);

            dot.DotMarked += OnDotMarked;
            dot.DotUnmarked += OnDotUnmarked;

            var x = X_OFFSET + column * Dot.Size.X - Dot.Size.X / 2;
            var y = Y_OFFSET + row * Dot.Size.Y - Dot.Size.Y / 2;

            dot.Position = new Point(x, y);
         
            _dots[row].Add(dot);
        }

        private void AddRoomAt(int row, int column)
        {
            var index = row * ROOM_COUNT + column;

            var room = new Room(index);

            var x = X_OFFSET + column * Room.Size.X;
            var y = Y_OFFSET + row * Room.Size.Y;

            room.Position = new Point(x, y);

            _rooms[row].Add(room);
        }

        protected override void LoadContent()
        {
            _dotSprite = Game.Content.Load<Texture2D>("barrel");
            _wallSprite = Game.Content.Load<Texture2D>("wall");
            _tileSprite = Game.Content.Load<Texture2D>("tile");
        }

        public override void Draw(GameTime gameTime)
        {
            DrawRooms();
            DrawConnections();
            DrawDots();
        }

        private void DrawDots()
        {
            foreach(var dotRow in _dots)
            {
                foreach (var dot in dotRow)
                {
                    _spriteBatch.Draw(_dotSprite, dot.Rectangle, dot.Color);
                }
            }
        }

        private void DrawConnections()
        {
            foreach (var dot in _dots.SelectMany(d => d))
            {
                foreach (var connection in dot.Connections)
                {
                    var origin = dot.Position.IsBefore(connection.Position) ? dot.Position : connection.Position;
                    var destination = dot.Position.IsBefore(connection.Position) ? connection.Position : dot.Position;

                    var angle = origin.AngleBetween(destination);

                    var drawOrigin = new Vector2
                    {
                        X = origin.X + Dot.Size.X / 2 + (angle == 0 ? 0 : 11),
                        Y = origin.Y + Dot.Size.Y / 2 + (angle != 0 ? 11 : -11)
                    };

                    _spriteBatch.Draw(_wallSprite, drawOrigin, ConnectionDrawRectangle, Color.White, (float)angle, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }
        
        static Rectangle ConnectionDrawRectangle => new(1, 1, 62, 22);

        private void DrawRooms()
        {
            foreach (var roomRow in _rooms)
            {
                foreach (var room in roomRow)
                {
                    _spriteBatch.Draw(_tileSprite, room.Rectangle, room.Color);
                }
            }
        }

        private void OnDotMarked(Dot dot)
        {
            if (currentDot == null)
            {
                currentDot = dot;
            }
            else
            {
                if (currentDot != dot)
                {
                    currentDot.Connections.Add(dot);
                    currentDot.Marked = false;
                    dot.Marked = false;
                    currentDot = null;
                }
            }
        }

        private void OnDotUnmarked(Dot dot)
        {
            if (currentDot != null && currentDot == dot)
            {
                currentDot = null;
            }
        }
    }
}
