using System.Collections;
using UnityEngine;

namespace Nixian.Waves
{
    public class LightRefraction : MonoBehaviour
    {

        public static Vector2 CalculateRefraction(Vector2 dir, Vector2 normal, float n1, float n2)
        {
            dir.Normalize();
            normal.Normalize();

            float eta = n1 / n2;
            float dotProd = Vector2.Dot(dir, normal);
            float k = 1.0f - eta * eta * (1.0f - dotProd * dotProd);

            if (k < 0)
            {
                return dir - 2 * dotProd * normal;
            }

            return eta * dir - (eta * dotProd + Mathf.Sqrt(k)) * normal;
        }
    }
}