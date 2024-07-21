using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Konnect.Main.Components.Dot
{
    internal class Dot
    {
        private const int TILE_SIZE = 64;

        public int Index { get; private set; }
        public Point Position { get; set; }
        public Color Color => Marked ? Color.PaleTurquoise : Color.White;

        public static Point Size => new(TILE_SIZE, TILE_SIZE);
        public Rectangle Rectangle => new(Position, Size);

        public List<Dot> Connections { get; private set; } = [];

        public bool Marked;

        public Dot(int index)
        {
            Index = index;
            MouseEvent.JustPressed += OnMouseJustPressed;
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
            }
        }
    }
}
