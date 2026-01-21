using System;
using System.Collections.Generic;
using UnityEngine;

    public class PathReqestManagr : MonoBehaviour 
    {
        public Queue<PathReqest> pathReqestsQueue = new Queue<PathReqest>();
        PathReqest curentPath;
        static PathReqestManagr instance;
        PathFinding pf;
        private bool _findingProces;

        private void Awake() 
        {
            instance = this;
            pf = GetComponent<PathFinding>();
        }
        public static void PathReqest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathReqest newPath = new PathReqest(pathStart, pathEnd, callback);
            instance.pathReqestsQueue.Enqueue(newPath);
            instance.TryPricesNext();
        }    
        void TryPricesNext()
        {
            if (!_findingProces && pathReqestsQueue.Count > 0)
            {
                curentPath = pathReqestsQueue.Dequeue();
                _findingProces = true;
                pf.StartFindPath(curentPath.pathStart, curentPath.pathEnd);
            }
        }
        public void FinishFindingProces(Vector3[] path, bool soucces)
        {
            curentPath.callback(path,soucces);
            _findingProces = false;
            instance.TryPricesNext();
        }

    }
    public struct PathReqest
    {
        public Vector3 pathStart; 
        public Vector3 pathEnd; 
        public Action<Vector3[], bool> callback;
        public PathReqest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            this.pathStart = pathStart; 
            this.pathEnd = pathEnd; 
            this.callback = callback;
        }
    
}