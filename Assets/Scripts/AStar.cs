using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    NodeGrid<Node> grid;

    public AStar(int width, int height, int depth, int nodeSize, Vector3 origin)
    {
        grid = new NodeGrid<Node>(width, height, depth, origin, (NodeGrid<Node> g, int x, int y, int z) => new Node(g, x, y, z));
    }
}
