using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Anchor : MonoBehaviour
{
    public float activationRadius = 2f;
    public int segments = 100; // Number of points in the circle
    public Color circleColor = Color.magenta;
    
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // Configure the LineRenderer properties
        lineRenderer.useWorldSpace = false; // Draw relative to the object
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = segments;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = circleColor;
        lineRenderer.endColor = circleColor;
        
        DrawCircle();
    }

    private void DrawCircle()
    {
        float angle = 0f;
        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * activationRadius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * activationRadius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }
}