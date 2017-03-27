using UnityEngine;
using System.Collections.Generic;
using Priority_Queue;

namespace Navmesh_Yyh
{
    public class GraphNode:FastPriorityQueueNode
    {
        public int Idx;
        public ConvexPolygon Poly;
        public List<int> Siblings = new List<int>();
        public PolyType Type;
        public float FValue;

        GraphNode(int index,ConvexPolygon polygon, PolyType t,List<int> siblings)
        {
            Idx = index;
            Poly = polygon;
            Type = t;
            Siblings = siblings;
        }
    }
}
