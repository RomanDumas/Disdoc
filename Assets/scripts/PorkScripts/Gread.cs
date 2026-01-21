using System.Collections.Generic;
using UnityEngine;
public class Gread : MonoBehaviour 
{
    public bool onDisplayGizmos;
    public LayerMask unwalkableMask;
    public Vector2 greadWorldSize;
    public float nodeRadius;
    Node[,] grid;
    float nodeDiameter;
    float greadSizeX, greadSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        greadSizeX = Mathf.RoundToInt(greadWorldSize.x / nodeDiameter);
        greadSizeY = Mathf.RoundToInt(greadWorldSize.y / nodeDiameter);
        CreateGread();
    }
    void CreateGread()
    {
        grid = new Node[(int)greadSizeX , (int)greadSizeY];
        Vector3 worldButtonLeft = transform.position - Vector3.right * greadWorldSize.x / 2 - Vector3.up * greadWorldSize.y / 2;
        for (int x = 0; x < greadSizeX; x++)
        {
            for (int y = 0; y < greadSizeY; y++)
            {
                Vector3 worldPosition = worldButtonLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up *(y * nodeDiameter + nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPosition, nodeRadius, unwalkableMask);
                
                grid[x,y] = new Node(walkable,worldPosition, x, y);
            }
        }
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + greadWorldSize.x/2) / greadWorldSize.x;
        float percentY = (worldPosition.y + greadWorldSize.y/2) / greadWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((greadSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((greadSizeY - 1) * percentY);
        return grid[x,y];
    } 

    public List<Node> FindNeighbors(int gX, int gY)
    {
        var Neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
        {
                if (x == 0 && y == 0) continue;
                int greadX = gX + x;
                int greadY = gY + y;

                if (greadX >= 0 && greadY >= 0 && greadX < greadSizeX && greadY < greadSizeY)
                {
                    Neighbors.Add(grid[greadX,greadY]);
                }
            }
        }
        return Neighbors;
    }
    public int MaxSize()
    {
        return (int)greadSizeX * (int)greadSizeY;
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(greadWorldSize.x,greadWorldSize.y,1));
        if (onDisplayGizmos && grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable? Color.white : Color.red;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter -.1f));
            }
        }
    }
}
