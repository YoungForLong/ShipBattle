using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Priority_Queue;

namespace Navmesh_Yyh
{
    public class PriorityPath<T>:IComparable
    {
        public int Priority;
        public List<T> Path = new List<T>();

        public int CompareTo(object obj)
        {
            PriorityPath<T> other = obj as PriorityPath<T>;

            return this.Priority > other.Priority ? 1 : -1;
        }
    }


    public class AStarSearch
    {
        Vector2 _start;
        Vector2 _end;

        int _startNode;
        int _endNode;

        NavmeshGraph _sourceMap;
        List<int> _route = new List<int>();

        List<PriorityPath<int>> _pathList = new List<PriorityPath<int>>();
        static int maxCachePathNum = 3;

        public AStarSearch(NavmeshGraph source)
        {
            _sourceMap = source;
            for(int i =0;i<source.Count +1;++i)
            {
                _route.Add(0);
            }
        }

        public bool BelongToPath(int start,int end,List<int> oldPath,List<int> ret)
        {
            int startTag = -1;
            int endTag = -1;
            for (int i = 0; i < oldPath.Count; ++i)
            {
                if (oldPath[i] == start)
                    startTag = i;

                if (oldPath[i] == end)
                    endTag = i;
            }

            if ((endTag != -1) && (startTag != -1))
            {
                int diff = endTag - startTag;
                int factor = diff / Mathf.Abs(diff);

                for (int j = startTag; !(j == endTag);)
                {
                    ret.Add(oldPath[j]);
                    j += factor;
                }

                return true;
            }

            return false;
        }

        public List<int> Result(Vector2 start,Vector2 end)
        {
            _start = start;
            _end = end;

            _startNode = _sourceMap.PointInWhichPoly(start);
            _endNode = _sourceMap.PointInWhichPoly(end);

            // 优化，如果在同一个多边形中，直接返回当前多边形
            var ret1 = new List<int>();
            if (_startNode == _endNode)
            {
                ret1.Add(_startNode);
                return ret1;
            }

            // 优化，如果被已经储存的路径包含，从路径中截取
            foreach (var iter in _pathList)
            {
                List<int> wayPoints = new List<int>();
                if (BelongToPath(_startNode, _endNode, iter.Path, wayPoints))
                {
                    iter.Priority++;
                    _pathList.Sort();
                    return wayPoints;
                }
                else
                {
                    iter.Priority--;
                }
            }


            search();

            Stack<int> path = new Stack<int>();
            int parent = _endNode;
            path.Push(parent);
            while (parent != _startNode)
            {
                parent = _route[parent];
                path.Push(parent);
            }

            // 反序列化
            List<int> ret = new List<int>();

            while (path.Count !=0)
            {
                int idx = path.Pop();
                ret.Add(idx);
            }

            // 优化，当前路径存入路径表中
            PriorityPath<int> pp = new PriorityPath<int>();
            pp.Path = ret;
            pp.Priority = 1;

            if (_pathList.Count >= maxCachePathNum)
            {
                var mpp = _pathList[_pathList.Count - 1];
                if (mpp.Priority < 1)
                {
                    _pathList.RemoveAt(_pathList.Count - 1);
                }
            }
            _pathList.Add(pp);
            _pathList.Sort();

            return ret;
        }

        bool search()
        {
            // 储存未被存入树中，但已经被访问过的节点
            FastPriorityQueue<GraphNode> OPEN = new FastPriorityQueue<GraphNode>(1000);

            // map的id从1开始
            int mapSize = _sourceMap.Count + 1;

            // 是否已经被访问且放入树中
            List<bool> CLOSE = new List<bool>();

            // 是否还在OPEN表中
            List<bool> isInOpen = new List<bool>();

            //初始化各种表
            for (int i =0; i<mapSize;++i)
            {
                CLOSE.Add(false);
                isInOpen.Add(false);
            }

            

            // 源节点入队
            if (!(_sourceMap.GetNodeById(_startNode).Poly.ContainsPoint(_start)))
            {
                _startNode = _sourceMap.PointInWhichPoly(_start);
            }

            var node = _sourceMap.GetNodeById(_startNode);
            OPEN.Enqueue(node, node.FValue);
            isInOpen[_startNode] = true;
            //_FValueArr.at(_startNode) = 0.0f;

            while (OPEN.Count != 0)
            {
                //F值最小的出队列
                //?var curNode = OPEN.First;
                OPEN.Dequeue();
                CLOSE.at(curNode->idx) = true;//所有出队列的节点，标记为CLOSE
                isInOpen.at(curNode->idx) = false;

                //找到目标节点，退出循环
                if (curNode->poly.containsPoint(_end))
                {
                    _endNode = curNode->idx;
                    return true;
                }

                //把相邻节点全部入队，前提是不在OPEN表中，不在CLOSE表中（未加入树）
                for (int i = 0; i < curNode->siblings.size(); ++i)
                {
                    auto next = _sourceMap.getNodeById(curNode->siblings.at(i));

                    if (CLOSE[next->idx])
                        continue;

                    if (isInOpen[next->idx] == false)
                    {
                        // 储存路径树的父节点id
                        _route[next->idx] = curNode->idx;

                        // 重新计算FValue
                        next->FValue = F_func(curNode, next);
                        next->FValue += curNode->FValue;

                        // 更新OPEN表
                        OPEN.push(next);
                        isInOpen.at(next->idx) = true;
                    }
                    else // 如果在OPEN表中，需要重新计算，比较是上一个父节点迭加小还是现在这个节点迭加起来小
                    {
                        float curFValue = next->FValue;
                        float newFValue = F_func(curNode, next);

                        if (newFValue < curFValue)
                        {
                            // 更换父节点和FValue
                            _route[next->idx] = curNode->idx;
                            next->FValue = newFValue;
                        }
                    }
                }// end for

            }// end while

            return false;
        }

        float F_func(GraphNode last, GraphNode cur)
        {

        }

        float H_func(Vector2 last,Vector2 cur)
        {

        }

        public static int left_or_right(Vector2 origin,Vector2 target)
        {

        }
    }
}
