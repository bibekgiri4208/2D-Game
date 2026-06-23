using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private SpriteShapeController _spriteShapeController;
    [SerializeField] private Transform _cameraTransform; // Drag your Main Camera here

    [Header("Level Settings")]
    [SerializeField] private float _xMultiplier = 2f;
    [SerializeField] private float _yMultiplier = 5f;
    [SerializeField, Range(0f, 1f)] private float _curveSmoothness = 0.5f;
    [SerializeField] private float _noiseStep = 0.1f;
    [SerializeField] private float _bottom = 20f;

    [Header("Infinite Generation Settings")]
    [SerializeField] private int _pointsAheadOfCamera = 30; // How many points to keep generated in front
    [SerializeField] private float _pointSpacing = 2f;       // Distance between each point on X axis

    private List<Vector3> _hillPoints = new List<Vector3>();
    private int _currentMaxPointIndex = 0;

    void Start()
    {
        if (_cameraTransform == null && Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }

        // Generate the initial set of points starting from X = 0
        for (int i = 0; i < _pointsAheadOfCamera; i++)
        {
            GenerateNextPoint();
        }

        RedrawSpline();
    }

    void Update()
    {
        if (_cameraTransform == null || _spriteShapeController == null) return;

        // Calculate the furthest X position currently generated
        float furthestGeneratedX = _currentMaxPointIndex * _pointSpacing;

        // If the camera gets close to the edge of what's generated, create a new point and remove an old one
        if (_cameraTransform.position.x + (_pointsAheadOfCamera * _pointSpacing) > furthestGeneratedX)
        {
            GenerateNextPoint();

            // Keep performance stable by removing points far behind the camera
            if (_hillPoints.Count > _pointsAheadOfCamera * 2)
            {
                _hillPoints.RemoveAt(0);
            }

            RedrawSpline();
        }
    }

    private void GenerateNextPoint()
    {
        // Continuous Perlin noise calculation using the absolute index pointer
        float noiseY = Mathf.PerlinNoise(0, _currentMaxPointIndex * _noiseStep) * _yMultiplier;
        Vector3 newPoint = new Vector3(_currentMaxPointIndex * _pointSpacing, noiseY, 0f);

        _hillPoints.Add(newPoint);
        _currentMaxPointIndex++;
    }

    private void RedrawSpline()
    {
        _spriteShapeController.spline.Clear();

        // 1. Draw all currently active rolling surface points
        for (int i = 0; i < _hillPoints.Count; i++)
        {
            _spriteShapeController.spline.InsertPointAt(i, _hillPoints[i]);

            // Apply bezier tangents to smooth out the hills
            if (i != 0 && i != _hillPoints.Count - 1)
            {
                _spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                _spriteShapeController.spline.SetLeftTangent(i, Vector3.left * _pointSpacing * _curveSmoothness);
                _spriteShapeController.spline.SetRightTangent(i, Vector3.right * _pointSpacing * _curveSmoothness);
            }
        }

        // 2. Safely close the bottom shape relative to current active visible points
        int totalSurfacePoints = _hillPoints.Count;
        Vector3 lastPoint = _hillPoints[totalSurfacePoints - 1];
        Vector3 firstPoint = _hillPoints[0];

        // Bottom Right corner anchor
        _spriteShapeController.spline.InsertPointAt(totalSurfacePoints, new Vector3(lastPoint.x, transform.position.y - _bottom, 0f));
        // Bottom Left corner anchor
        _spriteShapeController.spline.InsertPointAt(totalSurfacePoints + 1, new Vector3(firstPoint.x, transform.position.y - _bottom, 0f));

        // Rebuild the mesh layout bounds dynamically
        _spriteShapeController.RefreshSpriteShape();
    }
}