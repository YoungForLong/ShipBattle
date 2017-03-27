using UnityEngine;
using System.Collections;

namespace Navmesh_Yyh
{
    public class Triangle
    {
        public Vector2[] VertexArr = new Vector2[3];

        public Vector2 Centroid()
        {
            Vector2 sum = Vector2.zero;
            for (int i = 0; i < 3; ++i)
            {
                sum += VertexArr[i];
            }

            return sum / 3.0f;
        }

        public float Area()
        {
            float x1 = VertexArr[0].x;
            float y1 = VertexArr[0].y;
            float x2 = VertexArr[1].x;
            float y2 = VertexArr[1].y;
            float x3 = VertexArr[2].x;
            float y3 = VertexArr[2].y;

            return (x1 * y2 + y1 * x3 + y1 * x2 * y3 - x3 * y3 - x1 * y3 - x2 * y1);
        }
    }
}
