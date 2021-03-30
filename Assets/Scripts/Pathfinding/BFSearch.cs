using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSearch
{
    // Basic costs
    readonly int straightCost = 10;
    readonly int diagonalCost = 14;
    readonly int diagonal3DCost = 17;

    readonly NodeGrid<Node> grid;

    // List of nodes on frontier checked
    public List<Node> closedList;
    // List of nodes on frontier unchecked
    public List<Node> openList;

    public Node startNode;

    // Create the grid
    public BFSearch(int width, int height, int depth, int nodeSize, Vector3 origin)
    {
        grid = new NodeGrid<Node>(width, height, depth, nodeSize, origin, (NodeGrid<Node> g, int x, int y, int z) => new Node(g, x, y, z));
    }

    public NodeGrid<Node> GetGrid()
    {
        return grid;
    }

    // Find path between start and end coordinates
    public List<Node> FindPath(int startX, int startY, int startZ, int endX, int endY, int endZ)
    {
        startNode = grid.GetGridObject(startX, startY, startZ);
        Node endNode = grid.GetGridObject(endX, endY, endZ);

        closedList = new List<Node>();
        openList = new List<Node> { startNode };

        // Calculate g and f cost for all nodes
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int z = 0; z < grid.GetDepth(); z++)
                {
                    Node node = grid.GetGridObject(x, y, z);
                    // Make g cost infinite
                    node.g = int.MaxValue;
                    node.GetF();
                    // Clear data from previous path
                    node.lastNode = null;
                }
            }
        }

        startNode.g = 0;
        startNode.h = GetDistance(startNode, endNode);
        startNode.GetF();

        StepVisual.Instance.ClearSnapshots();
        StepVisual.Instance.TakeSnapshot(grid, startNode, openList, closedList);

        while (openList.Count > 0)
        {
            Node current = openList[0];

            if (current == endNode)
            {
                StepVisual.Instance.TakeSnapshot(grid, current, openList, closedList);
                StepVisual.Instance.FinalPathSnapshot(grid, GetPath(endNode));

                return (GetPath(endNode));
            }

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor)) continue;

                // If node is weighted, add move penalty
                if (neighbor.isWeighted)
                {
                    neighbor.g += 10;
                }

                // Only add to neighbor list if node is walkable
                if (!neighbor.isWalkable)
                {
                    closedList.Add(neighbor);
                    continue;
                }

                int tentativeG = current.g + GetDistance(current, neighbor);

                if (!openList.Contains(neighbor))
                {
                    neighbor.lastNode = current;
                    neighbor.g = tentativeG;
                    neighbor.h = GetDistance(neighbor, endNode);
                    neighbor.GetF();
                    openList.Add(neighbor);
                }

                StepVisual.Instance.TakeSnapshot(grid, current, openList, closedList);
            }

            openList.Remove(current);
            closedList.Add(current);
        }

        return null;
    }

    // Get the final path
    List<Node> GetPath(Node endNode)
    {
        List<Node> path = new List<Node>
        {
            endNode
        };
        Node current = endNode;

        // Retrace steps to starting node
        while (current.lastNode != startNode)
        {
            path.Add(current.lastNode);
            current = current.lastNode;
        }

        path.Reverse();

        return path;
    }

    // Add all neighbors to the neighbors list
    public List<Node> GetNeighbors(Node current)
    {
        List<Node> neighbors = new List<Node>();

        // Check if in range
        if (current.x - 1 >= 0)
        {
            // Left
            neighbors.Add(GetNode(current.x - 1, current.y, current.z));
            if (current.y - 1 >= 0)
            {
                //Left down
                neighbors.Add(GetNode(current.x - 1, current.y - 1, current.z));
            }
            if (current.y + 1 < grid.GetHeight())
            {
                // Left up
                neighbors.Add(GetNode(current.x - 1, current.y + 1, current.z));
            }
        }
        if (current.x + 1 < grid.GetWidth())
        {
            // Right
            neighbors.Add(GetNode(current.x + 1, current.y, current.z));
            if (current.y - 1 >= 0)
            {
                // Right down
                neighbors.Add(GetNode(current.x + 1, current.y - 1, current.z));
            }
            if (current.y + 1 < grid.GetHeight())
            {
                // Right up
                neighbors.Add(GetNode(current.x + 1, current.y + 1, current.z));
            }
        }
        if (current.y - 1 >= 0)
        {
            // Down
            neighbors.Add(GetNode(current.x, current.y - 1, current.z));
        }
        if (current.y + 1 < grid.GetHeight())
        {
            // Up
            neighbors.Add(GetNode(current.x, current.y + 1, current.z));
        }
        if (current.z - 1 >= 0)
        {
            // Back
            neighbors.Add(GetNode(current.x, current.y, current.z - 1));
            if (current.x - 1 >= 0)
            {
                // Back left
                neighbors.Add(GetNode(current.x - 1, current.y, current.z - 1));
                if (current.y - 1 >= 0)
                {
                    // Back left down
                    neighbors.Add(GetNode(current.x - 1, current.y - 1, current.z - 1));
                }
                if (current.y + 1 < grid.GetHeight())
                {
                    // Back left up
                    neighbors.Add(GetNode(current.x - 1, current.y + 1, current.z - 1));
                }
            }
            if (current.x + 1 < grid.GetWidth())
            {
                // Back right
                neighbors.Add(GetNode(current.x + 1, current.y, current.z - 1));
                if (current.y - 1 >= 0)
                {
                    // Back right down
                    neighbors.Add(GetNode(current.x + 1, current.y - 1, current.z - 1));
                }
                if (current.y + 1 < grid.GetHeight())
                {
                    // Back right up
                    neighbors.Add(GetNode(current.x + 1, current.y + 1, current.z - 1));
                }
            }
            if (current.y - 1 >= 0)
            {
                // Back down
                neighbors.Add(GetNode(current.x, current.y - 1, current.z - 1));
            }
            if (current.y + 1 < grid.GetHeight())
            {
                // Back up
                neighbors.Add(GetNode(current.x, current.y + 1, current.z - 1));
            }
        }
        if (current.z + 1 < grid.GetDepth())
        {
            // Forward
            neighbors.Add(GetNode(current.x, current.y, current.z + 1));
            if (current.x + 1 < grid.GetWidth())
            {
                // Forward right
                neighbors.Add(GetNode(current.x + 1, current.y, current.z + 1));
                if (current.y + 1 < grid.GetHeight())
                {
                    // Forward right up
                    neighbors.Add(GetNode(current.x + 1, current.y + 1, current.z + 1));
                }
                if (current.y - 1 >= 0)
                {
                    // Forward right down
                    neighbors.Add(GetNode(current.x + 1, current.y - 1, current.z + 1));
                }
            }
            if (current.x - 1 >= 0)
            {
                // Forward left
                neighbors.Add(GetNode(current.x - 1, current.y, current.z + 1));
                if (current.y + 1 < grid.GetHeight())
                {
                    // Forward left up
                    neighbors.Add(GetNode(current.x - 1, current.y + 1, current.z + 1));
                }
                if (current.y - 1 >= 0)
                {
                    // Forward left down
                    neighbors.Add(GetNode(current.x - 1, current.y - 1, current.z + 1));
                }
            }
            if (current.y + 1 < grid.GetHeight())
            {
                // Forward up
                neighbors.Add(GetNode(current.x, current.y + 1, current.z + 1));
            }
            if (current.y - 1 >= 0)
            {
                // Forward down
                neighbors.Add(GetNode(current.x, current.y - 1, current.z + 1));
            }
        }

        return neighbors;
    }

    public Node GetNode(int x, int y, int z)
    {
        return grid.GetGridObject(x, y, z);
    }

    // Get distance cost between start and end node
    int GetDistance(Node start, Node end)
    {
        int xDist = Mathf.Abs(start.x - end.x);
        int yDist = Mathf.Abs(start.y - end.y);
        int zDist = Mathf.Abs(start.z - end.z);
        List<int> distances = new List<int>
        {
            xDist, yDist, zDist
        };

        // Sort list (lowest to highest)
        distances.Sort();

        // Number of tiles to move on 3 axis at once
        int diagonal3D = distances[0];

        // Number of tiles to move on 2 axis at once
        int diagonal = distances[1] - distances[0];

        // Number of tiles to move on 1 axis
        int straight = distances[2] - (diagonal3D + diagonal);

        int finalDist = (diagonal3DCost * diagonal3D) + (diagonalCost * diagonal) + (straightCost * straight);

        return finalDist;
    }
}
