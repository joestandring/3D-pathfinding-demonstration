using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Draw a line between two specified positions (emulating Debug.Drawline)
    public LineRenderer DrawLine(Vector3 start, Vector3 end, Color color, float lineWidth = 0.1f)
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

    // Create a TextMesh game object at specified position
    public TextMesh DrawText(
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
}
