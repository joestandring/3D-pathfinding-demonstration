using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGrid : MonoBehaviour
{
    readonly int width = 4;
    readonly int height = 4;
    readonly int depth = 4;
    readonly int nodeSize = 10;
    readonly Vector3 origin = new Vector3(0, 0, 0);
    public GameObject cam;
    public NodeGrid<Node> nodeGrid;

    void Start()
    {
        // Create a grid with size 20x20x20 and node size of 10
        nodeGrid = new NodeGrid<Node>(width, height, depth, nodeSize, origin, (NodeGrid<Node> gameObject, int x, int y, int z) => new Node(gameObject, x, y, z));

        // Create main camera
        Vector3 position = new Vector3(width * -6, height * 18, depth * -6);
        Instantiate(cam, position, cam.transform.rotation);
    }
}
