using System;
using Microsoft.Xna.Framework;

namespace NotPong
{
    static class MyMath
    {
        public static float MoveTowards(float current, float target, float maxDelta)
        {
            if (MathF.Abs(target - current) <= maxDelta)
            {
                return target;
            }
            return current + MathF.Sign(target - current) * maxDelta;
        }

        public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
        {
            var toVector = target - current;

            float sqDist = toVector.X * toVector.X + toVector.Y * toVector.Y;

            if (sqDist == 0 || (maxDistanceDelta >= 0 && sqDist <= maxDistanceDelta * maxDistanceDelta))
                return target;

            float dist = (float)Math.Sqrt(sqDist);

            return new Vector2(
                current.X + toVector.X / dist * maxDistanceDelta,
                current.Y + toVector.Y / dist * maxDistanceDelta
            );
        }
    }
}
