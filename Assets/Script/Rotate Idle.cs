using UnityEngine;

public class RotateIdle : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("Degrees rotated per second")]
    public float rotationSpeed = 90f;

    [Header("Optional Bobbing (Floating up and down)")]
    public bool enableBobbing = true;
    public float bobbingSpeed = 2f;
    public float bobbingHeight = 0.15f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate around the Y axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Subtle vertical floating animation
        if (enableBobbing)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}