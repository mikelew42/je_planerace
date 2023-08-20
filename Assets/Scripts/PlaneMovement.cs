using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrust = 200f;
    public float responsiveness = 10f;
    public float throttle = 100;
    public float lift = 135;
    public float roll;
    public float pitch;
    public float yaw = 0;
    public Rigidbody rb;
    private float responseModifier
    {
        get {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary> This is how the AI and player interact with the plane movement system. yaw is only left here
    /// for testing in the editor, but should always be set to 0 with the actual movement system. </summary>
    public void HandeInputs(Vector2 rollAndPitch, float yaw)
    {
        roll = rollAndPitch.x;
        pitch = rollAndPitch.y;
        this.yaw = yaw;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle * CalculateSpeedMultiplier());
        //Debug.Log((transform.forward * maxThrust * throttle * CalculateSpeedMultiplier()).magnitude.ToString("F2"));
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(-transform.forward * roll * responseModifier);
        //rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        //rb.AddForce(transform.up * rb.velocity.magnitude * lift);
    }

    private static readonly float minSpeedMultiplier = 0.5f;
    private static readonly float maxSpeedMultiplier = 2f;

    private float CalculateSpeedMultiplier()
    {
        return Mathf.Lerp(maxSpeedMultiplier, minSpeedMultiplier,
            //normalizedOrientation
            Mathf.Clamp01((
            Vector3.Dot(transform.forward, Vector3.up) -
            Vector3.Dot(transform.forward, Vector3.down)
            ) * 0.5f + 0.5f));
    }
}
