using Microsoft.Xna.Framework;

namespace Konnect.Main.Components.Dot
{
    internal class Dot
    {
        private const int TILE_SIZE = 64;

        public int Index { get; private set; }
        public Point Position { get; set; }
        public Color Color { get; set; } = Color.White;

        public static Point Size => new(TILE_SIZE, TILE_SIZE);
        public Rectangle Rectangle => new(Position, Size);

        public Dot(int index)
        {
            Index = index;
        }
    }
}
