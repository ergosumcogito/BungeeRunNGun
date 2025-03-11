using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnchorConnector : MonoBehaviour
{
    public float detectionRadius = 2f;
    public float momentumSpeed = 10f;
    public float rotationSpeedDegreesPerSecond = 90f;

    public KeyCode rotateClockwiseKey = KeyCode.U;
    public KeyCode rotateCounterClockwiseKey = KeyCode.O;

    private Anchor currentAnchor;
    private bool attached = false;
    private float currentAngle;
    private float radius;
    private Rigidbody2D rb;
    private bool lastRotationClockwise = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!attached)
        {
            Anchor anchor = FindNearestAnchor();
            if (anchor != null && (Input.GetKeyDown(rotateClockwiseKey) || Input.GetKeyDown(rotateCounterClockwiseKey)))
            {
                AttachToAnchor(anchor);
                Vector2 radial = transform.position - anchor.transform.position;
                currentAngle = Mathf.Atan2(radial.y, radial.x) * Mathf.Rad2Deg;
                radius = radial.magnitude;
            }
        }
        else
        {
            if (Input.GetKey(rotateClockwiseKey))
            {
                currentAngle -= rotationSpeedDegreesPerSecond * Time.deltaTime;
                lastRotationClockwise = true;
            }
            if (Input.GetKey(rotateCounterClockwiseKey))
            {
                currentAngle += rotationSpeedDegreesPerSecond * Time.deltaTime;
                lastRotationClockwise = false;
            }

            Vector2 offset = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;
            transform.position = currentAnchor.transform.position + (Vector3)offset;

            // Calculate and draw momentum vector
            Vector2 radial = transform.position - currentAnchor.transform.position;
            Vector2 tangent = lastRotationClockwise 
                ? new Vector2(radial.y, -radial.x)  // Corrected clockwise tangent
                : new Vector2(-radial.y, radial.x); // Corrected counter-clockwise tangent

            Vector2 momentumVector = tangent.normalized * momentumSpeed;
            Debug.DrawLine(transform.position, transform.position + (Vector3)momentumVector, Color.red, 0.1f);

            if (!Input.GetKey(rotateClockwiseKey) && !Input.GetKey(rotateCounterClockwiseKey))
            {
                Detach();
            }
        }
    }

    private Anchor FindNearestAnchor()
    {
        Anchor[] anchors = FindObjectsOfType<Anchor>();
        Anchor nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (Anchor anchor in anchors)
        {
            float dist = Vector2.Distance(transform.position, anchor.transform.position);
            if (dist <= anchor.activationRadius && dist < minDistance)
            {
                minDistance = dist;
                nearest = anchor;
            }
        }
        return nearest;
    }

    private void AttachToAnchor(Anchor anchor)
    {
        attached = true;
        currentAnchor = anchor;
        rb.velocity = Vector2.zero;
    }

    private void Detach()
    {
        Vector2 radial = transform.position - currentAnchor.transform.position;
        Vector2 tangent = lastRotationClockwise 
            ? new Vector2(radial.y, -radial.x)  // Corrected clockwise tangent
            : new Vector2(-radial.y, radial.x); // Corrected counter-clockwise tangent

        Vector2 swingMomentum = tangent.normalized * momentumSpeed;

        TarodevController.PlayerController pc = GetComponent<TarodevController.PlayerController>();
        if (pc != null)
        {
            pc.externalMomentum = swingMomentum;
        }

        attached = false;
        currentAnchor = null;
    }
}
