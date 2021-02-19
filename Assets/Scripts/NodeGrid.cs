using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid<TGridObject>
{
    readonly TGridObject[,,] gridArray;
    readonly TextMesh[,,] textArray;

    Color drawColor = Color.white;

    // Constructs the array using given width, height, and depth
    public NodeGrid(int width, int height, int depth, int nodeSize, Vector3 origin, System.Func<TGridObject> createGridObject)
    {
        gridArray = new TGridObject[width, height, depth];
        textArray = new TextMesh[width, height, depth];

        // Cycle through 3D array to create world text and grid object
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(0); y++)
            {
                for (int z = 0; z < gridArray.GetLength(0); z++)
                {
                    // Create text
                    textArray[x, y, z] = DrawText(
                        gridArray[x, y, z]?.ToString(),
                        drawColor,
                        null,
                        GetPosition(x, y, z) + (new Vector3(nodeSize, nodeSize, nodeSize) / 2f)
                    );

                    // Create grid object
                    gridArray[x, y, z] = createGridObject();
                }
            }
        }

        // Draw grid of lines
        for (int y = 0; y <= gridArray.GetLength(0); y++)
        {
            for(int z = 0; z <= gridArray.GetLength(0); z++)
            {
                DrawLine(GetPosition(0, y, z), GetPosition(width, y, z), drawColor);
            }
        }
        for (int x = 0; x <= gridArray.GetLength(0); x++)
        {
            for (int z = 0; z <= gridArray.GetLength(0); z++)
            {
                DrawLine(GetPosition(x, 0, z), GetPosition(x, height, z), drawColor);
            }
        }
        for (int x = 0; x <= gridArray.GetLength(0); x++)
        {
            for (int y = 0; y <= gridArray.GetLength(0); y++)
            {
                DrawLine(GetPosition(x, y, 0), GetPosition(x, y, depth), drawColor);
            }
        }

        // Convert to grid position using nodeSize
        Vector3 GetPosition(int x, int y, int z)
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
        void SetValueXYZ(int x, int y, int z, TGridObject value)
        {
            gridArray[x, y, z] = value;
            textArray[x, y, z].text = gridArray[x, y, z].ToString();
        }

        // Sets the value of a node using worldposition
        void SetValueWorldPos(Vector3 worldPos, TGridObject value)
        {
            GetXYZ(worldPos, out int x, out int y, out int z);
            SetValueXYZ(x, y, z, value);
        }

        // Get the value of a node using provided xyz coords
        TGridObject GetValueXYZ(int x, int y, int z)
        {
            return gridArray[x, y, z];
        }

        // Get the value of a node using worldposition
        TGridObject GetValueWorldPos(Vector3 worldPos)
        {
            GetXYZ(worldPos, out int x, out int y, out int z);
            return GetValueXYZ(x, y, z);
        }

        // Create a TextMesh game object at specified position
        TextMesh DrawText(
            string text,
            Color color,
            Transform parent = null,
            Vector3 localPosition = default,
            int fontSize = 20
        )
        {
            GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));

            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.alignment = TextAlignment.Center;

            textMesh.tag = "Value";

            return textMesh;
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

        Debug.Log("Grid created with width: " + width + " height: " + height + " depth: " + depth);
    }
}
