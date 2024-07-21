using Microsoft.Xna.Framework;

namespace Konnect.Framework.Extensions
{
    public static class PointExtensions
    {
        public static double AngleBetween(this Point origin, Point target)
        {
            return Math.Atan2(target.Y - origin.Y, target.X - origin.X);
        }

        public static bool IsBefore(this Point origin, Point target)
        {
            return origin.Y < target.Y || origin.X < target.X;
        }
    }
}
