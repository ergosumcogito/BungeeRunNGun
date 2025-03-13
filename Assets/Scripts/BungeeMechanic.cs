using UnityEngine;

public class BungeeMechanic : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 180f;
    // Fixed swing impulse magnitude to be applied on release along the tangent
    public float swingImpulse = 5f;
    // Maximum allowed horizontal speed (x-axis) after swing release
    public float maxHorizontalSpeed = 10f;

    private Rigidbody2D rb;
    // Current anchor the player is attached to
    private BungeeAnchor currentAnchor = null;
    // Whether the player is currently rotating around the anchor
    private bool isRotating = false;
    // Rotation direction: -1 for clockwise (U key), 1 for counterclockwise (O key)
    private int rotationDirection = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (currentAnchor != null)
        {
            // Check rotation key inputs
            bool keyU = Input.GetKey(KeyCode.U);
            bool keyO = Input.GetKey(KeyCode.O);

            if (keyU)
            {
                // U key for clockwise rotation
                isRotating = true;
                rotationDirection = -1;
            }
            else if (keyO)
            {
                // O key for counterclockwise rotation
                isRotating = true;
                rotationDirection = 1;
            }
            else
            {
                // When rotation key is released, apply swing impulse
                if (isRotating)
                {
                    ReleaseBungee();
                    isRotating = false;
                    rotationDirection = 0;
                    // Optionally detach from anchor after release
                    currentAnchor = null;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (currentAnchor != null && isRotating)
        {
            // Rotate the player around the anchor point
            Vector2 anchorPos = currentAnchor.transform.position;
            Vector2 offset = rb.position - anchorPos;
            float angle = rotationSpeed * rotationDirection * Time.fixedDeltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);
            Vector2 newOffset = new Vector2(offset.x * cos - offset.y * sin, offset.x * sin + offset.y * cos);
            Vector2 newPos = anchorPos + newOffset;
            rb.MovePosition(newPos);
        }
    }

    // Applies a fixed swing impulse in the tangent direction, making the swing momentum predictable
    void ReleaseBungee()
    {
        if (currentAnchor != null)
        {
            Vector2 anchorPos = currentAnchor.transform.position;
            Vector2 offset = rb.position - anchorPos;
            Vector2 tangent = Vector2.zero;

            if (rotationDirection == -1)
            {
                // Clockwise: tangent vector rotated by -90 degrees
                tangent = new Vector2(offset.y, -offset.x);
            }
            else if (rotationDirection == 1)
            {
                // Counterclockwise: tangent vector rotated by +90 degrees
                tangent = new Vector2(-offset.y, offset.x);
            }
            tangent.Normalize();

            // Calculate tangent as before
            tangent.Normalize();

            // Add a minimum upward bias if the vertical component is too low
            float minUpward = 0.3f;
            if (tangent.y < minUpward)
            {
                tangent.y = minUpward;
                tangent.Normalize();
            }
            // Then proceed with setting velocity along tangent
            float currentTangentSpeed = Vector2.Dot(rb.linearVelocity, tangent);
            Vector2 nonTangentVelocity = rb.linearVelocity - tangent * currentTangentSpeed;
            Vector2 newVelocity = nonTangentVelocity + tangent * swingImpulse;
            rb.linearVelocity = newVelocity;
        }
    }

    // Detect entering the anchor's trigger area
    void OnTriggerEnter2D(Collider2D col)
    {
        BungeeAnchor anchor = col.GetComponent<BungeeAnchor>();
        if (anchor != null)
        {
            currentAnchor = anchor;
        }
    }

    // Detect exiting the anchor's trigger area
    void OnTriggerExit2D(Collider2D col)
    {
        BungeeAnchor anchor = col.GetComponent<BungeeAnchor>();
        if (anchor != null && anchor == currentAnchor)
        {
            if (isRotating)
            {
                ReleaseBungee();
            }
            currentAnchor = null;
            isRotating = false;
            rotationDirection = 0;
        }
    }
}
