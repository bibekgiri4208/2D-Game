using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target; // Drag your Car body here

    [Header("Follow Speed (Smoothness)")]
    [SerializeField] private float _smoothSpeedX = 5f; // How fast it catches up horizontally
    [SerializeField] private float _smoothSpeedY = 2f; // Slower vertical speed prevents motion sickness

    [Header("Offsets")]
    [SerializeField] private Vector2 _offset = new Vector2(3f, 1f); // Keeps the car slightly back and down
    [SerializeField] private float _cameraZ = -10f;                // Standard 2D camera depth

    void LateUpdate()
    {
        if (_target == null) return;

        // 1. Calculate the ideal target position including our custom offsets
        float targetX = _target.position.x + _offset.x;
        float targetY = _target.position.y + _offset.y;

        // 2. Separately interpolate (Lerp) X and Y for independent smoothness control
        float newX = Mathf.Lerp(transform.position.x, targetX, _smoothSpeedX * Time.deltaTime);
        float newY = Mathf.Lerp(transform.position.y, targetY, _smoothSpeedY * Time.deltaTime);

        // 3. Apply the final calculated position to the camera
        transform.position = new Vector3(newX, newY, _cameraZ);
    }
}