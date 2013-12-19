using Mogre;

namespace sma_ogre.utils
{
    class Utils
    {
        static public float distanceXZ(Vector3 a, Vector3 b)
        {
            float x = (a.x - b.x);
            float z = (a.z - b.z);
            return (float)System.Math.Sqrt(x * x + z * z);
        }
    }
}
