using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // This now measures the new expanded tiled width
            length = spriteRenderer.bounds.size.x;
        }
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // Simple loop check: if camera crosses past 1/3rd of the tile's width, 
        // shift the anchor safely ahead.
        if (temp > startPos + length / 3)
        {
            startPos += length / 3;
        }
        else if (temp < startPos - length / 3)
        {
            startPos -= length / 3;
        }
    }
}