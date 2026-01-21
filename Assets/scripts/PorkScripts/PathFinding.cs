using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

    public class PathFinding : MonoBehaviour 
    {
        PathReqestManagr ReqestManager;
        Gread gread;
        private void Awake() 
        {
            ReqestManager = GetComponent<PathReqestManagr>();
            gread = GetComponent<Gread>();
        }
        public void StartFindPath(Vector3 startPos, Vector3 targetPos)
        {
            StartCoroutine(FindPath(startPos, targetPos));
        }
        IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = gread.NodeFromWorldPoint(startPos);
            Node targetNode = gread.NodeFromWorldPoint(targetPos);

            if (startNode.walkable && targetNode.walkable)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                bool success = false;

                var openSet = new FastHeap<Node>(gread.MaxSize());
                var clouseSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node curentNode = openSet.RemoveFirst();
                    
                    clouseSet.Add(curentNode);

                    if(curentNode == targetNode)
                    {
                        sw.Stop();
                        print("path finding for " + sw.ElapsedMilliseconds + "ms");
                        success = true;
                        break;
                    }
                    
                    foreach (Node neighbor in gread.FindNeighbors(curentNode.greadX, curentNode.greadY))
                    {
                        if(!neighbor.walkable || clouseSet.Contains(neighbor)) continue; 
                        
                        int NewMovementCostToNeighber = curentNode.gCost + GetDistance(curentNode, neighbor);
                        if (NewMovementCostToNeighber < neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = NewMovementCostToNeighber;
                            neighbor.hCost = GetDistance(neighbor,targetNode);
                            neighbor.parent = curentNode;

                            if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                            else openSet.UpdateItem(neighbor);
                        }
                    }
                }
                if(success)
                {
                    Vector3[] path = RetractPath(startNode, targetNode);
                    ReqestManager.FinishFindingProces(path, success);
                }
            }
            yield return null;
        }
        private Vector3[] RetractPath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            Node curentNode = endNode;
            while (curentNode != startNode)
            {
                path.Add(curentNode);
                curentNode = curentNode.parent;
            }
            Vector3[] wayPoint = SimplyfyPath(path);
            Array.Reverse(wayPoint);
            return wayPoint;
        }
        Vector3[] SimplyfyPath(List<Node> path)
        {
            List<Vector3> wayPath = new List<Vector3>();
            Vector2 oldDir = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 newDir = new Vector2(path[i-1].greadX - path[i].greadX, path[i-1].greadY - path[i].greadY);
                if (newDir != oldDir)
                {
                    wayPath.Add(path[i].worldPosition);
                }
                oldDir = newDir;
            }
            return wayPath.ToArray();
        }
        public int GetDistance(Node n1, Node n2)
        {
            int distX = Mathf.Abs(n1.greadX - n2.greadX);
            int distY = Mathf.Abs(n1.greadY - n2.greadY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            
            return 14 * distX + 10 * (distY - distX);
            
        } 
}