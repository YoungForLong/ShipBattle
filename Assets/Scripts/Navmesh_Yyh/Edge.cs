using UnityEngine;
using System.Collections;

namespace Navmesh_Yyh
{
    public struct Edge
    {
        public Vector2 From;
        public Vector2 To;

        public Edge(Vector2 from_, Vector2 to_)
        {
            From = from_;
            To = to_;
        }

        public bool ContainsPoint(Vector2 point_)
        {
            var f2p = point_ - From;
            var t2p = point_ - To;

            return f2p.sqrMagnitude < 1 || t2p.sqrMagnitude < 1;
        }

        public static bool operator == (Edge lhs,Edge rhs)
        {
            bool tagA = (lhs.From == rhs.From && lhs.To == rhs.To);
            bool tagB = (lhs.From == rhs.To && lhs.To == rhs.From);

            return tagA || tagB;
        }

        public static bool operator !=(Edge lhs, Edge rhs)
        {
            bool tagA = (lhs.From == rhs.From && lhs.To == rhs.To);
            bool tagB = (lhs.From == rhs.To && lhs.To == rhs.From);

            return !(tagA || tagB);
        }
    }

}
