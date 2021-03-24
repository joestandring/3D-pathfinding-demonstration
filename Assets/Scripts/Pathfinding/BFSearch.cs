using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSearch
{
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
        //// Get starting node
        //Node startNode = grid.GetGridObject(startX, startY, startZ);
        //// Get last node
        //Node endNode = grid.GetGridObject(endX, endY, endZ);

        //closedList = new List<Node>();
        //frontier = new List<Node> { startNode };

        //// Get starting snapshot
        //StepVisual.Instance.ClearSnapshots();
        //StepVisual.Instance.TakeSnapshot(grid, startNode, frontier, closedList);

        //// cycle until open list empty
        //while (frontier.Count > 0)
        //{
        //    Node current = frontier[0];

        //    // Get path to last node if last node
        //    if (current == endNode)
        //    {
        //        // Take snapshot
        //        StepVisual.Instance.TakeSnapshot(grid, current, frontier, closedList);
        //        StepVisual.Instance.FinalPathSnapshot(grid, GetPath(endNode));

        //        return GetPath(endNode);
        //    }

        //    closedList.Add(current);
        //    frontier.Remove(current);

        //    foreach (Node neighbor in GetNeighbors(current))
        //    {
        //        // Only add to neighbor list if node is walkable
        //        if (!neighbor.isWalkable)
        //        {
        //            closedList.Add(neighbor);
        //            continue;
        //        }
        //        if (!frontier.Contains(neighbor))
        //        {
        //            frontier.Add(neighbor);
        //            neighbor.lastNode = current;
        //        }
        //        StepVisual.Instance.TakeSnapshot(grid, current, frontier, closedList);
        //    }
        //}

        //// Out of nodes
        //return null;

        startNode = grid.GetGridObject(startX, startY, startZ);
        Node endNode = grid.GetGridObject(endX, endY, endZ);

        closedList = new List<Node>();
        openList = new List<Node> { startNode };

        StepVisual.Instance.ClearSnapshots();
        StepVisual.Instance.TakeSnapshot(grid, startNode, openList, closedList);

        while (openList.Count > 0)
        {
            Node current = openList[0];

            if (current == endNode)
            {
                Debug.Log("FINAL NODE REACHED");

                StepVisual.Instance.TakeSnapshot(grid, current, openList, closedList);
                StepVisual.Instance.FinalPathSnapshot(grid, GetPath(endNode));

                return (GetPath(endNode));
            }

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor)) continue;

                // Only add to neighbor list if node is walkable
                if (!neighbor.isWalkable)
                {
                    closedList.Add(neighbor);
                    continue;
                }

                if (!openList.Contains(neighbor))
                {
                    neighbor.lastNode = current;
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
}

