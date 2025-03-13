using UnityEngine;

public class BungeeAnchor : MonoBehaviour
{
    // Custom radius for anchor activation
    public float activationRadius = 2.0f;
    
    // Option to draw radius in game using a LineRenderer
    public bool drawRadiusInGame = true;
    public int segments = 100; // Number of segments for circle drawing

    private LineRenderer lineRenderer;

    void Awake() {
        // Optionally create and setup LineRenderer for in-game drawing
        if (drawRadiusInGame) {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            // Setup LineRenderer properties
            lineRenderer.widthMultiplier = 0.1f;
            lineRenderer.loop = true;
            lineRenderer.positionCount = segments + 1;
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    void Update() {
        // Update in-game drawn circle if enabled
        if (drawRadiusInGame && lineRenderer != null) {
            DrawCircle();
        }
    }

    // Draw circle with LineRenderer
    private void DrawCircle() {
        float angle = 0f;
        for (int i = 0; i < segments + 1; i++) {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * activationRadius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * activationRadius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + transform.position);
            angle += 360f / segments;
        }
    }

    // Draw activation radius in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}