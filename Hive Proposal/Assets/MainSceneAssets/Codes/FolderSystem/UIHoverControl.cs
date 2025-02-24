using UnityEngine;

public class UIHoverControl : MonoBehaviour
{
    public GameObject hoverUI;

    private bool isHovered = false; // Track hover state

    private void Start()
    {
        if (hoverUI != null)
            hoverUI.SetActive(false); // Ensure UI starts hidden
    }

    public void OnHover()
    {
        if (!isHovered && hoverUI != null)
        {
            hoverUI.SetActive(true);
            isHovered = true;
        }
    }

    public void OnLookAway()
    {
        if (isHovered && hoverUI != null)
        {
            hoverUI.SetActive(false);
            isHovered = false;
        }
    }
}
