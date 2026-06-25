using UnityEngine;

public class CameraPreview : MonoBehaviour
{
    [Tooltip("Speed of the camera moving forward")]
    public float speed = 3f;

    void Update()
    {
        // Moves the camera to the right (forward in 2D) at a constant speed
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}