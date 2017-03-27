using UnityEngine;
using System.Collections.Generic;

namespace Navmesh_Yyh
{
    public class NavmeshGraph
    {
        Dictionary<int, GraphNode> _nodeMap;

        int _count = 0;

        public int Count
        {
            get { return _count; }
        }

        public NavmeshGraph()
        {
            _count = 0;
        }

        public bool LoadFromFile(string filename)
        {
            return false;
        }

        public void SaveToFile(string filename)
        {

        }

        public int PointInWhichPoly(Vector2 point)
        {
            int ret = 0;
            foreach(var pair in _nodeMap)
            {
                var node = pair.Value;
                if(node.Poly.ContainsPoint(point))
                {
                    ret = node.Idx;
                    break;
                }
            }

            if(ret != 0)
            {
                return ret;
            }
            else
            {
                MonoBehaviour.print(string.Format("cannot find this point in map, it's likely that inside the obstacle set. This point is %f, %f", point.x, point.y));
                return 0;
            }
        }

        public static bool LOS_test(Vector2 heading, Edge e)
        {
            Vector3 h3d = new Vector3(heading.x, heading.y, 0);
            Vector3 f3d = new Vector3(e.From.x, e.From.y, 0);
            Vector3 t3d = new Vector3(e.To.x, e.To.y, 0);

            Vector3 test_a = Vector3.zero;
            Vector3 test_b = Vector3.zero;

            test_a = Vector3.Cross(f3d, h3d);
            test_b = Vector3.Cross(t3d, h3d);

            bool tag_a = test_a.z > 0;
            bool tag_b = test_b.z > 0;

            return tag_a == tag_b;
        }

        public GraphNode GetNodeById(int idx)
        {
            return _nodeMap[idx];
        }

        public void AddPoly(GraphNode node)
        {
            _nodeMap.Add(node.Idx, node);
            _count++;
        }

        public void SliceEdge(Vector2 from, Vector2 to)
        {
            
        }

        public void CombineEdge(Vector2 from,Vector2 to)
        {

        }
    }
}
