using Microsoft.Xna.Framework;

namespace Konnect.Main.GameComponents
{
    internal class Room(int index)
    {
        private const int TILE_SIZE = 64;

        public int Index { get; private set; } = index;
        public Point Position { get; set; }
        public bool Closed { get; set; }
        public Color Color => Closed ? Color.DarkTurquoise : Color.White;
        public static Point Size => new(TILE_SIZE, TILE_SIZE);
        public Rectangle Rectangle => new(Position, Size);
    }
}
