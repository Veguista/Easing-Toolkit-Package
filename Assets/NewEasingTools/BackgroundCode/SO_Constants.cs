using UnityEngine;

namespace SecondOrderEasing
{
    public struct SO_Constants
    {
        internal float w, z, d, k1, k2, k3;        // Constants.

        public SO_Constants(float f, float z, float r)
        {
            // Compute constants.
            w = 2 * Mathf.PI * f;
            this.z = z;
            d = w * Mathf.Sqrt(Mathf.Abs(z * z - 1));
            k1 = z / (Mathf.PI * f);
            k2 = 1 / (w * w);
            k3 = r * z / w;
        }
    }
}

