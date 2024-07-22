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

        readonly List<List<Dot>> _dots = [];
        readonly int _boardSize;
        readonly SpriteBatch _spriteBatch;

        Dot currentDot;

        Texture2D _dotSprite, _wallSprite, _tileSprite;

        public Board(Game game) : base(game)
        {
            _spriteBatch = game.Services.GetService<SpriteBatch>();

            _boardSize = 11;

            for (var i = 0; i < _boardSize; i++)
            {
                _dots.Add([]);
                for (var j = 0; j < _boardSize; j++)
                {
                    var dot = new Dot(i * _boardSize + j);
                    dot.DotMarked += OnDotMarked;
                    dot.DotUnmarked += OnDotUnmarked;
                    _dots[i].Add(dot);
                }
            }
        }

        protected override void LoadContent()
        {
            _dotSprite = Game.Content.Load<Texture2D>("barrel");
            _wallSprite = Game.Content.Load<Texture2D>("wall");
            _tileSprite = Game.Content.Load<Texture2D>("tile");
        }

        public override void Draw(GameTime gameTime)
        {
            DrawBaseBackground();
            DrawConnections();
            DrawDots();
        }

        private void DrawDots()
        {
            foreach (var dot in _dots.SelectMany(d => d))
            {
                var x = X_OFFSET + dot.Index / _boardSize * Dot.Size.X - Dot.Size.X / 2;
                var y = Y_OFFSET + dot.Index % _boardSize * Dot.Size.Y - Dot.Size.Y / 2;

                dot.Position = new Point(x, y);

                _spriteBatch.Draw(_dotSprite, dot.Rectangle, dot.Color);
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

                    _spriteBatch.Draw(_wallSprite, drawOrigin, new Rectangle(1, 1, 62, 22), Color.White, (float)angle, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        private void DrawBaseBackground()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var position = new Vector2(X_OFFSET + i * _tileSprite.Width, Y_OFFSET + j * _tileSprite.Height);
                    _spriteBatch.Draw(_tileSprite, position, null, Color.White);
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
