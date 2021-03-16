using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creation of basic grid 
public class MakeGrid : MonoBehaviour
{
    readonly Utils utils;

    readonly int width = 5;
    readonly int height = 5;
    readonly int depth = 5;
    readonly int nodeSize = 10;
    readonly Vector3 origin = Vector3.zero;
    public GameObject cam;
    public NodeGrid<Node> nodeGrid;
    AStar aStar;
    [SerializeField] StepVisual stepVisual;

    void Start()
    {
        // Create a grid with size 20x20x20 and node size of 10
        //nodeGrid = new NodeGrid<Node>(width, height, depth, nodeSize, origin, (NodeGrid<Node> gameObject, int x, int y, int z) => new Node(gameObject, x, y, z));

        // A*
        aStar = new AStar(width, height, depth, nodeSize, origin);

        // Create main camera
        Vector3 position = new Vector3(width * -6, height * 18, depth * -6);
        Instantiate(cam, position, cam.transform.rotation);

        NodeGrid<Node> grid = aStar.GetGrid();

        // Create visual nodes
        stepVisual.Setup(grid);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Node> path = aStar.FindPath(0, 0, 0, 4, 4, 4);
            if (path != null)
            {
                foreach (Node node in path)
                {
                    utils.DrawLine(
                        new Vector3(node.y, node.y, node.z) * nodeSize + Vector3.one * nodeSize / 2,
                        new Vector3(node.x, node.y, node.z) * nodeSize + Vector3.one * nodeSize / 2,
                        Color.white
                    );
                }
            }
        }
    }
}
