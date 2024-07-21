using Konnect.Main.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Konnect.Main.Components
{
    internal class Board
    {
        readonly List<List<Dot>> _dots = [];
        readonly int _boardSize;

        Dot currentDot;

        public Board(int boardSize)
        {
            _boardSize = boardSize;

            for (var i = 0; i < boardSize; i++)
            {
                _dots.Add([]);
                for (var j = 0; j < boardSize; j++)
                {
                    var dot = new Dot(i * boardSize + j);
                    dot.DotMarked += OnDotMarked;
                    dot.DotUnmarked += OnDotUnmarked;
                    _dots[i].Add(dot);
                }
            }
        }

        public void Draw(SpriteBatch batch, Texture2D dot, Texture2D wall, Vector2? offset = null)
        {
            if (offset == null)
                offset = Vector2.Zero;

            DrawConnections(batch, wall);
            DrawDots(batch, dot, offset);
        }

        private void DrawDots(SpriteBatch batch, Texture2D sprite, Vector2? offset)
        {
            foreach (var dot in _dots.SelectMany(d => d))
            {
                var x = offset.Value.X + (dot.Index / _boardSize) * Dot.Size.X - Dot.Size.X / 2;
                var y = offset.Value.Y + (dot.Index % _boardSize) * Dot.Size.Y - Dot.Size.Y / 2;

                dot.Position = new Point((int)x, (int)y);

                batch.Draw(sprite, dot.Rectangle, dot.Color);
            }
        }

        private void DrawConnections(SpriteBatch batch, Texture2D wall)
        {
            foreach(var dot in _dots.SelectMany(d => d))
            {
                foreach(var connection in dot.Connections)
                {
                    var origin = dot.Position.IsBefore(connection.Position) ? dot.Position : connection.Position;
                    var destination = dot.Position.IsBefore(connection.Position) ? connection.Position : dot.Position;

                    var angle = origin.AngleBetween(destination);

                    var drawOrigin = new Vector2
                    {
                        X = (origin.X + Dot.Size.X / 2) + (angle == 0 ? 0 : 11),
                        Y = (origin.Y + Dot.Size.Y / 2) + (angle != 0 ? 11 : -11)
                    };

                    batch.Draw(wall, drawOrigin, new Rectangle(1, 1, 62, 22), Color.White, (float) angle, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                }
            }
        }

        private void OnDotMarked(Dot dot)
        {
            if(currentDot == null)
            {
                currentDot = dot;
            }
            else
            {
                if(currentDot != dot)
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
            if(currentDot != null && currentDot == dot)
            {
                currentDot = null;
            }
        }
    }
}
