using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 0.2f; // Adjust for desired scrolling speed
    private Renderer rend;
    public float offset; // Start at the top of the UV
    public float currentOffset;
    public float normalisedScrollProgress;

    void Start()
    {
        currentOffset = offset;

        rend = GetComponent<Renderer>();
        rend.material.mainTextureOffset = new Vector2(0, currentOffset);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            currentOffset += scroll * scrollSpeed;
            currentOffset = Mathf.Clamp(currentOffset, 0f, offset);
            rend.material.mainTextureOffset = new Vector2(0, currentOffset);

            float minValue = 0f;
            float maxValue = offset;
            normalisedScrollProgress = (currentOffset - minValue) / (maxValue - minValue);
        }
    }
}
