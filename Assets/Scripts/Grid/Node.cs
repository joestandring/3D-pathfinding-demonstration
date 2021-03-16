using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node containing data in the grid
public class Node
{
#pragma warning disable IDE0052 // Remove unread private members
    readonly NodeGrid<Node> grid;
#pragma warning restore IDE0052 // Remove unread private members
    public int x;
    public int y;
    public int z;

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

    // Calculate the f cost
    public void GetF()
    {
        f = g + h;
    }

    public override string ToString()
    {
        return x + ", " + y + ", " + z;
    }
}
