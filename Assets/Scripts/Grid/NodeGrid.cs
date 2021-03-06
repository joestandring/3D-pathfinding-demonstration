﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The grid object, containing data in each "Node"
public class NodeGrid<TGridObject>
{
    readonly int width;
    readonly int height;
    readonly int depth;
    readonly int nodeSize;
    Vector3 origin;

    readonly TGridObject[,,] gridArray;
    //readonly TextMesh[,,] textArray;

    Color drawColor = Color.white;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
    }

    // Constructs the array using given width, height, and depth
    public NodeGrid(int width, int height, int depth, int nodeSize, Vector3 origin, Func<NodeGrid<TGridObject>, int, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.origin = origin;
        this.nodeSize = nodeSize;

        gridArray = new TGridObject[width, height, depth];
        //textArray = new TextMesh[width, height, depth];

        // Cycle through 3D array to create world text and grid object
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    // Create text
                    //textArray[x, y, z] = utils.DrawText(
                    //    "0",
                    //    drawColor,
                    //    null,
                    //    GetPosition(x, y, z) + (new Vector3(nodeSize, nodeSize, nodeSize) / 2f)
                    //);

                    // Create grid object
                    gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }

        // Draw grid of lines
        for (int y = 0; y <= gridArray.GetLength(1); y++)
        {
            for(int z = 0; z <= gridArray.GetLength(2); z++)
            {
                DrawLine(GetPosition(0, y, z), GetPosition(width, y, z), drawColor);
            }
        }
        for (int x = 0; x <= gridArray.GetLength(0); x++)
        {
            for (int z = 0; z <= gridArray.GetLength(2); z++)
            {
                DrawLine(GetPosition(x, 0, z), GetPosition(x, height, z), drawColor);
            }
        }
        for (int x = 0; x <= gridArray.GetLength(0); x++)
        {
            for (int y = 0; y <= gridArray.GetLength(1); y++)
            {
                DrawLine(GetPosition(x, y, 0), GetPosition(x, y, depth), drawColor);
            }
        }
    }

    public void TriggerGridObjectChanged(int x, int y, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y, z = z });
    }

    // Convert to grid position using nodeSize
    public Vector3 GetPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * nodeSize + origin;
    }

    // Convert world position to XYZ coordinates
    void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / nodeSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / nodeSize);
        z = Mathf.FloorToInt((worldPosition - origin).z / nodeSize);
    }

    // Sets the value of a node using provided xyz coords
    //void SetValueXYZ(int x, int y, int z, TGridObject value)
    //{
    //    gridArray[x, y, z] = value;
    //    textArray[x, y, z].text = gridArray[x, y, z].ToString();
    //}

    // Sets the value of a node using worldposition
    //void SetValueWorldPos(Vector3 worldPos, TGridObject value)
    //{
    //    GetXYZ(worldPos, out int x, out int y, out int z);
    //    SetValueXYZ(x, y, z, value);
    //}

    // Get the value of a node using provided xyz coords
    //TGridObject GetValueXYZ(int x, int y, int z)
    //{
    //    return gridArray[x, y, z];
    //}

    // Get the value of a node using worldposition
    //TGridObject GetValueWorldPos(Vector3 worldPos)
    //{
    //    GetXYZ(worldPos, out int x, out int y, out int z);
    //    return GetValueXYZ(x, y, z);
    //}

    // Gets the object at a specified location
    public TGridObject GetGridObject(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 00 && x < width && y < height && z < depth)
        {
            return gridArray[x, y, z];
        }
        else
        {
            return default;
        }
    }
    public TGridObject GetGridObject(Vector3 worldPos)
    {
        GetXYZ(worldPos, out int x, out int y, out int z);
        return GetGridObject(x, y, z);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetDepth()
    {
        return depth;
    }

    public int GetNodeSize()
    {
        return nodeSize;
    }

    // Draw a line between two specified positions (emulating Debug.Drawline)
    LineRenderer DrawLine(Vector3 start, Vector3 end, Color color, float lineWidth = 0.1f)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Unlit/Texture"));
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.tag = "Line";

        return (lineRenderer);
    }
}
