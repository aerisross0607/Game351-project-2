using UnityEngine;

public class HovercraftController : MonoBehaviour
{
    public Terrain terrain;

    [Header("Movement")]
    public float acceleration = 25f;
    public float maxSpeed = 20f;
    public float turnSpeed = 80f;

    [Header("Hovering")]
    public float hoverHeight = 1.5f;
    public float hoverForce = 10f;
    public float quiverAmount = 0.15f;
    public float quiverSpeed = 3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Hovercraft needs a Rigidbody!");
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        MoveHovercraft();
        TurnHovercraft();
        KeepAboveTerrain();
    }

    void MoveHovercraft()
    {
        // W = forward, S = backward
        float input = Input.GetAxis("Vertical");
        Debug.Log("Vertical input: " + input);

        Vector3 force =
            transform.forward * input * acceleration;

        rb.AddForce(force, ForceMode.Acceleration);

        // Limit horizontal speed
        Vector3 horizontalVelocity = new Vector3(
            rb.velocity.x,
            0f,
            rb.velocity.z
        );

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity =
                horizontalVelocity.normalized * maxSpeed;

            rb.velocity = new Vector3(
                horizontalVelocity.x,
                rb.velocity.y,
                horizontalVelocity.z
            );
        }
    }

    void TurnHovercraft()
    {
        // A = left, D = right
        float input = Input.GetAxis("Horizontal");

        if (Mathf.Abs(input) > 0.01f)
        {
            float rotation =
                input * turnSpeed * Time.fixedDeltaTime;

            Quaternion turnRotation =
                Quaternion.Euler(0f, rotation, 0f);

            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    void KeepAboveTerrain()
    {
        if (terrain == null)
            return;

        Vector3 position = transform.position;

        float terrainHeight =
            terrain.SampleHeight(position)
            + terrain.transform.position.y;

        float quiver =
            Mathf.Sin(Time.time * quiverSpeed) * quiverAmount;

        float desiredHeight =
            terrainHeight + hoverHeight + quiver;

        float heightDifference =
            desiredHeight - position.y;

        // Push the hovercraft toward its desired height
        rb.AddForce(
            Vector3.up * heightDifference * hoverForce,
            ForceMode.Acceleration
        );
    }
}