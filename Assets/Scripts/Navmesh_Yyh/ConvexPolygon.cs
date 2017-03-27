using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Navmesh_Yyh
{
    public class ConvexPolygon
    {
        public List<Vector2> VertexArr = new List<Vector2>();
        public Vector2 Centroid;

        public ConvexPolygon(params Vector2[] args)
        {
            foreach (var arg in args)
            {
                VertexArr.Add(arg);
            }

            if (!checkConvex())
            {
                VertexArr.Clear();
                MonoBehaviour.print("it's not a convex polygon");
            }

            calculateCentroid();
        }

        public ConvexPolygon(List<Vector2> argList)
        {
            foreach (var arg in argList)
            {
                VertexArr.Add(arg);
            }

            if (!checkConvex())
            {
                VertexArr.Clear();
                MonoBehaviour.print("it's not a convex polygon");
            }

            calculateCentroid();
        }

        public List<Triangle> Divide()
        {
            List<Triangle> ret = new List<Triangle>();
            Vector2 first_vert = VertexArr[0];

            int count = 0;
            while (true)
            {
                Triangle t = new Triangle();
                t.VertexArr[0] = first_vert;
                t.VertexArr[1] = VertexArr[count++];

                if (count < VertexArr.Count)
                    break;
                t.VertexArr[2] = VertexArr[count];

                ret.Add(t);
            }

            return ret;
        }

        bool checkConvex()
        {
            if (VertexArr.Count < 3)
                return false;

            if (VertexArr.Count == 3)
            {
                Vector2 ab = VertexArr[1] - VertexArr[0];
                Vector2 ac = VertexArr[2] - VertexArr[0];

                float diff = ac.y / ab.y - ac.x / ac.x;

                if (diff < 0.01f)
                    return true;
                else
                    return false;
            }

            bool tagZ = tri_point_z(VertexArr[0], VertexArr[1], VertexArr[2]);
            for (int i = 3; i < VertexArr.Count; ++i)
            {
                if (tri_point_z(VertexArr[i - 2], VertexArr[i - 1], VertexArr[i]) != tagZ)
                {
                    return false;
                }
            }

            //倒数第二个点和最后一个点
            var last = VertexArr[VertexArr.Count - 1];
            var lastbutone = VertexArr[VertexArr.Count - 2];

            bool tag = tri_point_z(lastbutone, last, VertexArr[0]);
            if (tag != tagZ)
                return false;

            tag = tri_point_z(last, VertexArr[0], VertexArr[1]);
            if (tag != tagZ)
                return false;

            return true;
        }

        bool tri_point_z(Vector2 p1, Vector2 p2, Vector2 p3)
        {

            var e1 = p2 - p1;
            var e2 = p3 - p2;
            Vector3 ret = Vector3.Cross(new Vector3(e1.x, e1.y, 0), new Vector3(e2.x, e2.y, 0));

            return ret.z > 0;
        }

        public bool ContainsPoint(Vector2 p)
        {
            var length = VertexArr.Count;

            //第一次检测
            bool lastTest = tri_point_z(p, VertexArr[0], VertexArr[1]);
            bool curTest = false;
            for (int i = 1; i < length - 1; ++i)
            {
                curTest = tri_point_z(p, VertexArr[i], VertexArr[i + 1]);
                if (curTest != lastTest)
                    return false;
                lastTest = curTest;
            }

            //最后一次检测
            curTest = tri_point_z(p, VertexArr[length - 1], VertexArr[0]);
            if (curTest != lastTest)
                return false;

            return true;
        }

        public List<Edge> GetAllEdges()
        {
            List<Edge> edgeArr = new List<Edge>();

            var curVert = VertexArr[0];

            for (int i = 1; i < VertexArr.Count; ++i)
            {
                Edge e = new Edge(curVert, VertexArr[i]);

                edgeArr.Add(e);
            }

            Edge last = new Edge(VertexArr[VertexArr.Count - 1], VertexArr[0]);
            edgeArr.Add(last);

            return edgeArr;
        }

        public Edge FindCommonEdge(ConvexPolygon other)
        {
            List<Vector2> edge = new List<Vector2>();
            for (int i = 0; i < VertexArr.Count; ++i)
            {
                for (int j = 0; j < other.VertexArr.Count; ++j)
                {
                    if ((VertexArr[i] - other.VertexArr[j]).sqrMagnitude < 1)
                    {
                        edge.Add(VertexArr[i]);
                    }
                }
            }

            return new Edge(edge[0], edge[1]);
        }

        void calculateCentroid()
        {
            Vector2 sumWeight = Vector2.zero;
            float sumA = 0.0f;

            var tris = this.Divide();

            foreach (var tri in tris)
            {
                Vector2 eachC = tri.Centroid();
                float eachArea = tri.Area();

                sumA += eachArea;
                sumWeight += eachC * eachArea;
            }

            Centroid = sumWeight / sumA;
        }
    }
}
