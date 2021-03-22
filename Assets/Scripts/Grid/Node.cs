using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node containing data in the grid
public class Node
{
    readonly NodeGrid<Node> grid;

    public int x;
    public int y;
    public int z;

    // Travel cost
    public int g;
    // Heuristic
    public int h;
    // total cost (g + h)
    public int f;

    public bool isWalkable;
    public Node lastNode;

    public Node(NodeGrid<Node> grid, int x, int y, int z)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
        isWalkable = true;
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

    // Toggle if node is walkable
    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y, z);
    }
}
