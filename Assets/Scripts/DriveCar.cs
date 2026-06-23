using System.Collections.Generic;
using UnityEngine;

public class DriveCar : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _carRB; // Added this line to fix the error!
    [SerializeField] private Rigidbody2D _frontTireRB;
    [SerializeField] private Rigidbody2D _backTireRB;
    [SerializeField] private float _speed = 150f;
    [SerializeField] private float _roationSpeed = 300f; // Note: You can fix the typo here if you want (_rotationSpeed)

    private float _moveInput;

    private void Update()
    {
        // Gets keyboard input (A/D, Left/Right arrows) or controller joystick input
        _moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // Applies rotational force (torque) to the wheels for consistent physics movement
        _frontTireRB.AddTorque(-_moveInput * _speed * Time.fixedDeltaTime);
        _backTireRB.AddTorque(-_moveInput * _speed * Time.fixedDeltaTime);
        _carRB.AddTorque(-_moveInput * _roationSpeed * Time.fixedDeltaTime);
    }
}