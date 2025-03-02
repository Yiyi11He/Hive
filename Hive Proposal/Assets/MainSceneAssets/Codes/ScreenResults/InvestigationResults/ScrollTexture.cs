using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeed = 0.2f; // Adjust for desired scrolling speed
    private Renderer rend;
    public float offset = 0.93f; // Start at the top of the UV

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.mainTextureOffset = new Vector2(0, offset);
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // Reversed scroll direction
            offset += scroll * scrollSpeed;
            offset = Mathf.Clamp(offset, 0f, 0.93f);
            rend.material.mainTextureOffset = new Vector2(0, offset);
        }
    }
}
