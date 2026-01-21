using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public int greadX;
    public int greadY;

    public int gCost;
    public int hCost;
    public Vector3 worldPosition;
    public Node parent;
    private int heapIndex;
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }
    

    public Node(bool _walkable, Vector3 _worldPos, int gX, int gY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        greadX = gX;
        greadY = gY;
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    /////////////////////
    public bool NodeComparison(Node n = null)
    {
        if(n == null || this == n) return false;

        if (fCost > n.fCost) return true;
        else if (fCost == n.fCost)
        {
            return hCost > n.hCost;
        }
        return false;
    }
    /////////////////////

    public int CompareTo(Node other)
    {
        int compere;
        compere = fCost.CompareTo(other.fCost);
        if(compere == 0)
        {
            compere = hCost.CompareTo(other.hCost);
        }
        return -compere;
    }
}