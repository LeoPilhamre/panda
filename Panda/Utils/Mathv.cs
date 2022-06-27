namespace Panda.Utils
{

    internal static class Mathv
    {

        public static float Remap(float v, float a, float b, float c, float d)
        {
            return c + (v - a) * (d - c) / (b - a);
        }

    }

}