using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Shows each step of a pathfinding algorithm by colouring nodes
public class StepVisual : MonoBehaviour
{
    public static StepVisual Instance { get; private set; }
    [SerializeField] Transform pfPathfindingDebugStepVisualNode;
    Transform[,,] visualNodeArray;
    List<Transform> visualNodeList;
    List<GridSnapshotAction> gridSnapshotActions;
    bool autoShowSnapshots;
    float snapshotTime;

    public Material nodeDefault;
    public Material nodeClosed;
    public Material nodeSearched;
    public Material nodeSearching;
    public Material nodeWall;

    int snapshotIndex = 0;

    public Control control;

    void Awake()
    {
        Instance = this;
        visualNodeList = new List<Transform>();
        gridSnapshotActions = new List<GridSnapshotAction>();
    }

    void Update()
    {
        // Go to previous snapshot
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ShowPreviousSnapshot();
        }

        // Advance one snapshot
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ShowNextSnapshot();
        }

        // Go to first snapshot
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ShowFirstSnapshot();
        }

        // Go to last snapshot
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ShowLastSnapshot();
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            autoShowSnapshots = true;
        }

        // Run through all snapshots
        if (autoShowSnapshots)
        {
            float timerMax = .05f;
            snapshotTime -= Time.deltaTime;
            if (snapshotTime <= 0f)
            {
                snapshotTime += timerMax;
                ShowNextSnapshot();
                if (snapshotIndex == gridSnapshotActions.Count - 1)
                {
                    autoShowSnapshots = false;
                }
            }
        }
    }

    // Set up the grid of visual nodes
    public void Setup(NodeGrid<Node> grid)
    {
        // Get grid dimensions
        visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight(), grid.GetDepth()];

        // Make a visual node at each grid coordinate
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int z = 0; z < grid.GetDepth(); z++)
                {
                    Vector3 gridPos = new Vector3(x, y, z) * grid.GetNodeSize() + Vector3.one * grid.GetNodeSize() * .5f;
                    Transform visualNode = CreateVisualNode(gridPos);
                    visualNodeArray[x, y, z] = visualNode;
                    visualNodeList.Add(visualNode);
                }
            }
        }

        HideVisuals();
    }

    // Create a visual node object
    Transform CreateVisualNode(Vector3 position)
    {
        Transform visualNodeTransform = Instantiate(pfPathfindingDebugStepVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    // Hides the node visuals
    void HideVisuals()
    {
        foreach (Transform visualNode in visualNodeList)
        {
            SetupVisualNode(visualNode, 9999, 9999, 9999);
        }
    }

    // Create the text fields for the visual nodes
    void SetupVisualNode(Transform visualNodeTransform, int g, int h, int f)
    {
        if (f < 1000)
        {
            visualNodeTransform.GetChild(0).GetComponent<TextMeshPro>().SetText(f.ToString());
            visualNodeTransform.GetChild(1).GetComponent<TextMeshPro>().SetText(g.ToString());
            visualNodeTransform.GetChild(2).GetComponent<TextMeshPro>().SetText(h.ToString());
        }
        else
        {
            visualNodeTransform.GetChild(0).GetComponent<TextMeshPro>().SetText("");
            visualNodeTransform.GetChild(1).GetComponent<TextMeshPro>().SetText("");
            visualNodeTransform.GetChild(2).GetComponent<TextMeshPro>().SetText("");
        }
    }

    // Advances to the next snapshoy
    void ShowNextSnapshot()
    {
        if (snapshotIndex < gridSnapshotActions.Count - 1)
        {
            snapshotIndex++;
            GridSnapshotAction gridSnapshotAction = gridSnapshotActions[snapshotIndex];
            gridSnapshotAction.TriggerAction();
            control.snapshotStatus.text = "Snapshot index: " + (snapshotIndex + 1) + "/" + gridSnapshotActions.Count;
        }
    }

    // Shows the previous snapshot
    void ShowPreviousSnapshot()
    {
        if (snapshotIndex > 0)
        {
            snapshotIndex--;
            GridSnapshotAction gridSnapshotAction = gridSnapshotActions[snapshotIndex];
            gridSnapshotAction.TriggerAction();
            control.snapshotStatus.text = "Snapshot index: " + (snapshotIndex + 1) + "/" +gridSnapshotActions.Count;
        }
    }

    // Go to first snapshot
    void ShowFirstSnapshot()
    {
        snapshotIndex = 0;
        GridSnapshotAction gridSnapshotAction = gridSnapshotActions[snapshotIndex];
        gridSnapshotAction.TriggerAction();
        control.snapshotStatus.text = "Snapshot index: " + (snapshotIndex + 1) + "/" + gridSnapshotActions.Count;
    }

    // Go to last snapshot
    void ShowLastSnapshot()
    {
        snapshotIndex = gridSnapshotActions.Count - 1;
        GridSnapshotAction gridSnapshotAction = gridSnapshotActions[snapshotIndex];
        gridSnapshotAction.TriggerAction();
        control.snapshotStatus.text = "Snapshot index: " + (snapshotIndex + 1) + "/" + gridSnapshotActions.Count;
    }

    public void ClearSnapshots()
    {
        gridSnapshotActions.Clear();
    }

    // Take a snapshot of the current state of the grid and add it to the list
    public void TakeSnapshot
    (
        NodeGrid<Node> grid,
        Node current,
        List<Node> openList,
        List<Node> closedList
    )
    {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideVisuals);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int z = 0; z < grid.GetDepth(); z++)
                {
                    Node node = grid.GetGridObject(x, y, z);

                    int g = node.g;
                    int h = node.h;
                    int f = node.f;

                    Vector3 gridPos = new Vector3(node.x, node.y, node.z) *
                    grid.GetNodeSize() +
                    Vector3.one *
                    grid.GetNodeSize() *
                    5f;

                    bool isCurrent = node == current;
                    bool isInOpenList = openList.Contains(node);
                    bool isInClosedList = closedList.Contains(node);

                    int tmpX = x;
                    int tmpY = y;
                    int tmpZ = z;

                    gridSnapshotAction.AddAction(() =>
                    {
                        Transform visualNode = visualNodeArray[tmpX, tmpY, tmpZ];
                        SetupVisualNode(visualNode, g, h, f);

                        Material material = nodeDefault;

                        if (isInClosedList)
                        {
                            material = nodeClosed;
                        }
                        if (isInOpenList)
                        {
                            material = nodeSearched;
                        }
                        if (isCurrent)
                        {
                            material = nodeSearching;
                        }

                        visualNode.GetComponent<MeshRenderer>().material = material;
                    });
                }
            }
        }

        gridSnapshotActions.Add(gridSnapshotAction);
    }

    // Get a snapshot of the final path
    public void FinalPathSnapshot(NodeGrid<Node> grid, List<Node> path)
    {
        GridSnapshotAction gridSnapshotAction = new GridSnapshotAction();
        gridSnapshotAction.AddAction(HideVisuals);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                for (int z = 0; z < grid.GetDepth(); z++)
                {
                    Node node = grid.GetGridObject(x, y, z);

                    int g = node.g;
                    int h = node.h;
                    int f = node.f;

                    Vector3 gridPos = new Vector3(node.x, node.y, node.z) *
                    grid.GetNodeSize() +
                    Vector3.one *
                    grid.GetNodeSize() *
                    5f;

                    bool isInPath = path.Contains(node);

                    int tmpX = x;
                    int tmpY = y;
                    int tmpZ = z;

                    gridSnapshotAction.AddAction(() =>
                    {
                        Transform visualNode = visualNodeArray[tmpX, tmpY, tmpZ];
                        SetupVisualNode(visualNode, g, h, f);

                        Material material;

                        if (isInPath)
                        {
                            material = nodeSearching;
                        }
                        else
                        {
                            material = nodeDefault;
                        }

                        visualNode.GetComponent<MeshRenderer>().material = material;
                    });
                }
            }
        }

        gridSnapshotActions.Add(gridSnapshotAction);
    }

    class GridSnapshotAction
    {
        Action action;

        public GridSnapshotAction()
        {
            action = () => { };
        }

        public void AddAction(Action action)
        {
            this.action += action;
        }

        public void TriggerAction()
        {
            action();
        }
    }
}
