using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGrid : MonoBehaviour
{
    void Start()
    {
        // Create a grid with size 20x20x20 and node size of 10
        NodeGrid grid = new NodeGrid(5, 5, 5, 10);
    }
}
