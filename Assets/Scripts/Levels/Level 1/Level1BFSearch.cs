using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

// Level 1 - 2D grid
public class Level1BFSearch : MonoBehaviour
{
    readonly int width = 10;
    readonly int height = 10;
    readonly int depth = 1;
    readonly int nodeSize = 10;
    Vector3 origin = Vector3.zero;
    public Camera oldCam;
    public Camera cam;
    public NodeGrid<Node> nodeGrid;
    BFSearch bFSearch;
    [SerializeField] StepVisual stepVisual;
    public Control control;
    public GameObject wallObject;
    public GameObject weightedObject;
    public GameObject goalObject;
    NodeGrid<Node> bFSearchGrid;
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

        SetupBFSearch();
    }

    void Update()
    {
        // Dijkstra
        if (Input.GetKeyDown(KeyCode.Space) && currentAlgorithm == "bFSearch")
        {
            currentAlgorithm = null;

            control.snapshotStatus.text = "Running Breadth-First-Search pathfinding algorithm...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Node> path = bFSearch.FindPath(startX, startY, startZ, goalX, goalY, goalZ);
            stopwatch.Stop();
            control.snapshotStatus.text = "Press ENTER to move to next algorithm\nPathfinding complete!";

            long timeToComplete = stopwatch.ElapsedMilliseconds;
            int pathedNodes = path.Count;
            int openListSize = bFSearch.openList.Count;
            int closedListSize = bFSearch.closedList.Count;
            int totalNodesSearched = openListSize + closedListSize;
            int totalMoveCost = path[path.Count - 1].g;

            UnityEngine.Debug.Log($"Time taken to complete: {timeToComplete}ms");
            UnityEngine.Debug.Log($"Number of pathed nodes: {pathedNodes}");
            UnityEngine.Debug.Log($"Total nodes searched: {totalNodesSearched}");
            UnityEngine.Debug.Log($"Open list size: {openListSize}");
            UnityEngine.Debug.Log($"Closed list size: {closedListSize}");
            UnityEngine.Debug.Log($"Total move cost of path (g): {totalMoveCost}");

            control.consoleText.text = "Breadth-First-Search results\n\n"
            + "Time to complete: " + timeToComplete + "ms\n"
            + "Number of pathed nodes: " + pathedNodes + "\n"
            + "Total nodes searched: " + totalNodesSearched + "\n"
            + "Open list size: " + openListSize + "\n"
            + "Closed list size: " + closedListSize + "\n"
            + "Total move cost of path (g): " + totalMoveCost;

            // Write test results to file
            writer.WriteLine($"BFSearch, {timeToComplete}, {pathedNodes}, {totalNodesSearched}, {openListSize}, {closedListSize}, {totalMoveCost}");
            writer.Close();

            UnityEngine.Debug.Log($"Results written to: {pathString}");

            currentAlgorithm = "bFSearchDone";
        }

        if (Input.GetKeyDown(KeyCode.Return) && currentAlgorithm == "bFSearchDone")
        {
            currentAlgorithm = null;

            SceneManager.LoadScene("Level 1 Dijkstra");
        }
    }

    void SetupBFSearch()
    {
        bFSearch = new BFSearch(width, height, depth, nodeSize, origin);

        bFSearchGrid = bFSearch.GetGrid();

        // Add walls manually
        bFSearch.GetNode(2, 2, 0).SetWalkable(!bFSearch.GetNode(2, 2, 0).isWalkable);
        bFSearch.GetNode(2, 3, 0).SetWalkable(!bFSearch.GetNode(2, 3, 0).isWalkable);
        bFSearch.GetNode(2, 4, 0).SetWalkable(!bFSearch.GetNode(2, 4, 0).isWalkable);
        bFSearch.GetNode(3, 4, 0).SetWalkable(!bFSearch.GetNode(3, 4, 0).isWalkable);
        bFSearch.GetNode(4, 4, 0).SetWalkable(!bFSearch.GetNode(4, 4, 0).isWalkable);
        bFSearch.GetNode(5, 4, 0).SetWalkable(!bFSearch.GetNode(5, 4, 0).isWalkable);
        bFSearch.GetNode(6, 4, 0).SetWalkable(!bFSearch.GetNode(6, 4, 0).isWalkable);
        bFSearch.GetNode(6, 3, 0).SetWalkable(!bFSearch.GetNode(6, 3, 0).isWalkable);
        bFSearch.GetNode(6, 2, 0).SetWalkable(!bFSearch.GetNode(6, 2, 0).isWalkable);
        bFSearch.GetNode(1, 9, 0).SetWalkable(!bFSearch.GetNode(1, 9, 0).isWalkable);
        bFSearch.GetNode(1, 8, 0).SetWalkable(!bFSearch.GetNode(1, 8, 0).isWalkable);
        bFSearch.GetNode(3, 7, 0).SetWalkable(!bFSearch.GetNode(3, 7, 0).isWalkable);
        bFSearch.GetNode(4, 7, 0).SetWalkable(!bFSearch.GetNode(4, 7, 0).isWalkable);
        bFSearch.GetNode(5, 7, 0).SetWalkable(!bFSearch.GetNode(5, 7, 0).isWalkable);
        bFSearch.GetNode(6, 7, 0).SetWalkable(!bFSearch.GetNode(6, 7, 0).isWalkable);
        bFSearch.GetNode(7, 7, 0).SetWalkable(!bFSearch.GetNode(7, 7, 0).isWalkable);
        bFSearch.GetNode(8, 7, 0).SetWalkable(!bFSearch.GetNode(8, 7, 0).isWalkable);

        // Add weighted tiles manually
        bFSearch.GetNode(0, 4, 0).SetWeighted(!bFSearch.GetNode(0, 4, 0).isWeighted);
        bFSearch.GetNode(1, 4, 0).SetWeighted(!bFSearch.GetNode(1, 4, 0).isWeighted);

        // Draw walls
        for (int x = 0; x < bFSearchGrid.GetWidth(); x++)
        {
            for (int y = 0; y < bFSearchGrid.GetHeight(); y++)
            {
                for (int z = 0; z < bFSearchGrid.GetDepth(); z++)
                {
                    Node node = bFSearchGrid.GetGridObject(x, y, z);

                    // If the node is not walkable draw a wall
                    if (!node.isWalkable)
                    {
                        Object.Instantiate(wallObject, bFSearchGrid.GetPosition(x, y, z) + new Vector3(5, 5, 5), new Quaternion());
                    }

                    if (node.isWeighted)
                    {
                        Object.Instantiate(weightedObject, bFSearchGrid.GetPosition(x, y, z) + new Vector3(5, 5, 5), new Quaternion());
                    }
                }
            }
        }

        // Create visual nodes
        stepVisual.Setup(bFSearchGrid);

        // Show goal node
        Object.Instantiate(goalObject, bFSearchGrid.GetPosition(goalX, goalY, goalZ) + new Vector3(5, 5, 5), new Quaternion());
        Object.Instantiate(goalObject, bFSearchGrid.GetPosition(startX, startY, startZ) + new Vector3(5, 5, 5), new Quaternion());

        currentAlgorithm = "bFSearch";
    }
}

