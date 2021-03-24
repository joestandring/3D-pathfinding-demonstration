using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

// Creation of basic grid 
public class BFSearchTest : MonoBehaviour
{
    readonly int width = 6;
    readonly int height = 6;
    readonly int depth = 6;
    readonly int nodeSize = 10;
    readonly Vector3 origin = Vector3.zero;
    public GameObject cam;
    public NodeGrid<Node> nodeGrid;
    BFSearch bFSearch;
    [SerializeField] StepVisual stepVisual;
    public Control control;
    public GameObject wallObject;
    public GameObject goalObject;
    NodeGrid<Node> grid;
    readonly int startX = 0;
    readonly int startY = 0;
    readonly int startZ = 0;
    readonly int goalX = 3;
    readonly int goalY = 3;
    readonly int goalZ = 3;

    void Start()
    {
        // A*
        bFSearch = new BFSearch(width, height, depth, nodeSize, origin);

        // Create main camera
        Vector3 position = new Vector3(width * -6, height * 18, depth * -6);
        Instantiate(cam, position, cam.transform.rotation);

        grid = bFSearch.GetGrid();

        // Add walls manually
        //bFSearch.GetNode(2, 2, 2).SetWalkable(!bFSearch.GetNode(2, 2, 2).isWalkable);

        // Draw walls
        DrawWalls();

        // Create visual nodes
        stepVisual.Setup(grid);

        // Show goal node
        Object.Instantiate(goalObject, grid.GetPosition(goalX, goalY, goalZ) + new Vector3(5, 5, 5), new Quaternion());
        Object.Instantiate(goalObject, grid.GetPosition(startX, startY, startZ) + new Vector3(5, 5, 5), new Quaternion());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            control.snapshotStatus.text = "Running Best-First-Search pathfinding algorithm...";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Node> path = bFSearch.FindPath(startX, startY, startZ, goalX, goalY, goalZ);
            stopwatch.Stop();
            control.snapshotStatus.text = "Pathfinding complete!";

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

            control.consoleText.text = "RESULTS\n\n"
            + "Time to complete: " + timeToComplete + "ms\n"
            + "Number of pathed nodes: " + pathedNodes + "\n"
            + "Total nodes searched: " + totalNodesSearched + "\n"
            + "Open list size: " + openListSize + "\n"
            + "Closed list size: " + closedListSize + "\n"
            + "Total move cost of path (g): " + totalMoveCost;

            // Create file at "Results/path_name/DDMMYYYY"
            string scene = SceneManager.GetActiveScene().name;
            string time = System.DateTimeOffset.Now.ToString("yyyyMMdd");
            string pathString = $"Results/{scene}/{time}.csv";
            StreamWriter writer = new StreamWriter(pathString, true);

            // Write test results to file
            writer.WriteLine("'Algorythm', 'Time to complete (ms)', 'Number of pathed nodes', 'Total nodes searched', 'Open list size', 'Closed list size', 'Total move cost (g)'");
            writer.WriteLine($"BFSearch, {timeToComplete}, {pathedNodes}, {totalNodesSearched}, {openListSize}, {closedListSize}, {totalMoveCost}");
            writer.Close();
            UnityEngine.Debug.Log($"Results written to: {pathString}");
        }
    }

    // Draw walls on unwalkable nodes
    void DrawWalls()
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int z = 0; z < grid.GetDepth(); z++)
                {
                    Node node = grid.GetGridObject(x, y, z);

                    // If the node is not walkable draw a wall
                    if (!node.isWalkable)
                    {
                        Object.Instantiate(wallObject, grid.GetPosition(x, y, z) + new Vector3(5, 5, 5), new Quaternion());
                    }
                }
            }
        }
    }
}
