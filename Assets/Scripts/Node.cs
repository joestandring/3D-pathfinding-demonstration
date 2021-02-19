using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    NodeGrid<Node> grid;
    int x;
    int y;
    int z;

    // Travel cost
    public int g;
    // Heuristic
    public int h;
    // total cost (g + h)
    public int f;

    public Node lastNode;

    public Node(NodeGrid<Node> grid, int x, int y, int z)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return x + ", " + y + ", " + z;
    }
}
