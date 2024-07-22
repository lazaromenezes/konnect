using Konnect.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Konnect.Main.GameComponents
{
    internal class Dot
    {
        private const int TILE_SIZE = 64;

        public int Index { get; private set; }
        public Point Position { get; set; }
        public bool Marked { get; set; }

        public List<Dot> Connections { get; private set; } = [];

        public delegate void DotMarkHandler(Dot dot);
        public event DotMarkHandler DotMarked;
        public event DotMarkHandler DotUnmarked;

        public Color Color => Marked ? Color.PaleTurquoise : Color.White;
        public static Point Size => new(TILE_SIZE, TILE_SIZE);
        public Rectangle Rectangle => new(Position, Size);

        public Dot(int index)
        {
            Index = index;
            MouseEvents.JustPressed += OnMouseJustPressed;
        }

        public bool IsAdjacent(Dot other)
        {
            return Math.Abs(Position.X - other.Position.X + Position.Y - other.Position.Y) == TILE_SIZE;
        }

        private bool WasClicked(Point mousePosition)
        {
            return Rectangle.Contains(mousePosition);
        }

        private void OnMouseJustPressed(Point mousePosition)
        {
            if (WasClicked(mousePosition))
            {
                Marked = !Marked;
                if (Marked)
                {
                    DotMarked.Invoke(this);
                }
                else
                {
                    DotUnmarked.Invoke(this);
                }
            }
        }
    }
}
