using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pork : MonoBehaviour
{
    public Transform target;
        public float speed = 5f;
        Vector3[] path;
        int targetIndex;
        
        private void Update() {
            if(Input.GetKeyUp(KeyCode.P))
            {
                PathReqestManagr.PathReqest(transform.position,target.position,OnPathFound);
            }
        }
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if(pathSuccessful)
            {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
                
            }
        }
        IEnumerator FollowPath()
        {
            Vector3 curentTargetPoint = path[0];
            targetIndex = 0;
            while (true)
            {
                if (transform.position == curentTargetPoint)
                {
                    targetIndex++;
                    if(targetIndex >= path.Length)
                    {
                        yield break;
                    } 
                    curentTargetPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, curentTargetPoint,speed * Time.fixedDeltaTime);
                yield return null;
            }

        }
        void OnDrawGizmos()
        {
            if(path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i],Vector3.one * 0.5f);
                    if (i == targetIndex)
                        Gizmos.DrawLine(transform.position, path[i]);
                    else 
                        Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
