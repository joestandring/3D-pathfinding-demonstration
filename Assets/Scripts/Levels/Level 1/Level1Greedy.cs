using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

// Level 1 - 2D grid
public class Level1Greedy : MonoBehaviour
{
    readonly int width = 10;
    readonly int height = 10;
    readonly int depth = 1;
    readonly int nodeSize = 10;
    Vector3 origin = Vector3.zero;
    public Camera oldCam;
    public Camera cam;
    public NodeGrid<Node> nodeGrid;
    Greedy greedy;
    [SerializeField] StepVisual stepVisual;
    public Control control;
    public GameObject wallObject;
    public GameObject weightedObject;
    public GameObject goalObject;
    NodeGrid<Node> greedyGrid;
    readonly int startX = 0;
    readonly int startY = 0;
    readonly int startZ = 0;
    readonly int goalX = 9;
    readonly int goalY = 9;
    readonly int goalZ = 0;
    string pathString;
    StreamWriter writer;
    string currentAlgorithm = null;
    Vector3 camPosition;

    void Start()
    {
        // Write exprement metadata
        pathString = $"Results/Level1Results.csv";
        writer = new StreamWriter(pathString, true);

        // Create main camera
        camPosition = new Vector3(10 * (width / 2), 10 * (width / 2), depth - 100);
        Instantiate(cam, camPosition, new Quaternion());
        oldCam.enabled = false;
        control.SetupNewCamera();
        cam.enabled = true;

        SetupGreedy();
    }

    void Update()
    {
        // Greedy
        if (Input.GetKeyDown(KeyCode.Space) && currentAlgorithm == "greedy")
        {
            currentAlgorithm = null;

            control.snapshotStatus.text = "Running Greedy Best-First-Search pathfinding algorithm...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Node> path = greedy.FindPath(startX, startY, startZ, goalX, goalY, goalZ);
            stopwatch.Stop();
            control.snapshotStatus.text = "Press ENTER to move to next algorithm\nPathfinding complete!";

            long timeToComplete = stopwatch.ElapsedMilliseconds;
            int pathedNodes = path.Count;
            int openListSize = greedy.openList.Count;
            int closedListSize = greedy.closedList.Count;
            int totalNodesSearched = openListSize + closedListSize;
            int totalMoveCost = path[path.Count - 1].g;

            UnityEngine.Debug.Log($"Time taken to complete: {timeToComplete}ms");
            UnityEngine.Debug.Log($"Number of pathed nodes: {pathedNodes}");
            UnityEngine.Debug.Log($"Total nodes searched: {totalNodesSearched}");
            UnityEngine.Debug.Log($"Open list size: {openListSize}");
            UnityEngine.Debug.Log($"Closed list size: {closedListSize}");
            UnityEngine.Debug.Log($"Total move cost of path (g): {totalMoveCost}");

            control.consoleText.text = "Greedy results\n\n"
            + "Time to complete: " + timeToComplete + "ms\n"
            + "Number of pathed nodes: " + pathedNodes + "\n"
            + "Total nodes searched: " + totalNodesSearched + "\n"
            + "Open list size: " + openListSize + "\n"
            + "Closed list size: " + closedListSize + "\n"
            + "Total move cost of path (g): " + totalMoveCost;

            // Write test results to file
            writer.WriteLine($"Greedy, {timeToComplete}, {pathedNodes}, {totalNodesSearched}, {openListSize}, {closedListSize}, {totalMoveCost}");
            writer.Close();

            UnityEngine.Debug.Log($"Results written to: {pathString}");

            currentAlgorithm = "greedyDone";
        }
    }

    void SetupGreedy()
    {
        greedy = new Greedy(width, height, depth, nodeSize, origin);

        greedyGrid = greedy.GetGrid();

        // Add walls manually
        greedy.GetNode(2, 2, 0).SetWalkable(!greedy.GetNode(2, 2, 0).isWalkable);
        greedy.GetNode(2, 3, 0).SetWalkable(!greedy.GetNode(2, 3, 0).isWalkable);
        greedy.GetNode(2, 4, 0).SetWalkable(!greedy.GetNode(2, 4, 0).isWalkable);
        greedy.GetNode(3, 4, 0).SetWalkable(!greedy.GetNode(3, 4, 0).isWalkable);
        greedy.GetNode(4, 4, 0).SetWalkable(!greedy.GetNode(4, 4, 0).isWalkable);
        greedy.GetNode(5, 4, 0).SetWalkable(!greedy.GetNode(5, 4, 0).isWalkable);
        greedy.GetNode(6, 4, 0).SetWalkable(!greedy.GetNode(6, 4, 0).isWalkable);
        greedy.GetNode(6, 3, 0).SetWalkable(!greedy.GetNode(6, 3, 0).isWalkable);
        greedy.GetNode(6, 2, 0).SetWalkable(!greedy.GetNode(6, 2, 0).isWalkable);
        greedy.GetNode(1, 9, 0).SetWalkable(!greedy.GetNode(1, 9, 0).isWalkable);
        greedy.GetNode(1, 8, 0).SetWalkable(!greedy.GetNode(1, 8, 0).isWalkable);
        greedy.GetNode(3, 7, 0).SetWalkable(!greedy.GetNode(3, 7, 0).isWalkable);
        greedy.GetNode(4, 7, 0).SetWalkable(!greedy.GetNode(4, 7, 0).isWalkable);
        greedy.GetNode(5, 7, 0).SetWalkable(!greedy.GetNode(5, 7, 0).isWalkable);
        greedy.GetNode(6, 7, 0).SetWalkable(!greedy.GetNode(6, 7, 0).isWalkable);
        greedy.GetNode(7, 7, 0).SetWalkable(!greedy.GetNode(7, 7, 0).isWalkable);
        greedy.GetNode(8, 7, 0).SetWalkable(!greedy.GetNode(8, 7, 0).isWalkable);

        // Add weighted tiles manually
        greedy.GetNode(0, 4, 0).SetWeighted(!greedy.GetNode(0, 4, 0).isWeighted);
        greedy.GetNode(1, 4, 0).SetWeighted(!greedy.GetNode(1, 4, 0).isWeighted);

        // Draw walls
        for (int x = 0; x < greedyGrid.GetWidth(); x++)
        {
            for (int y = 0; y < greedyGrid.GetHeight(); y++)
            {
                for (int z = 0; z < greedyGrid.GetDepth(); z++)
                {
                    Node node = greedyGrid.GetGridObject(x, y, z);

                    // If the node is not walkable draw a wall
                    if (!node.isWalkable)
                    {
                        Object.Instantiate(wallObject, greedyGrid.GetPosition(x, y, z) + new Vector3(5, 5, 5), new Quaternion());
                    }

                    if (node.isWeighted)
                    {
                        Object.Instantiate(weightedObject, greedyGrid.GetPosition(x, y, z) + new Vector3(5, 5, 5), new Quaternion());
                    }
                }
            }
        }

        // Create visual nodes
        stepVisual.Setup(greedyGrid);

        // Show goal node
        Object.Instantiate(goalObject, greedyGrid.GetPosition(goalX, goalY, goalZ) + new Vector3(5, 5, 5), new Quaternion());
        Object.Instantiate(goalObject, greedyGrid.GetPosition(startX, startY, startZ) + new Vector3(5, 5, 5), new Quaternion());

        currentAlgorithm = "greedy";
    }
}
