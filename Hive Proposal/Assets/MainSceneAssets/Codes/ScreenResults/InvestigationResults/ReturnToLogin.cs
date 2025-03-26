using UnityEngine;

public class ReturnToLogin : MonoBehaviour
{
    [Header("Pages to Hide")]
    [SerializeField] private GameObject[] sensitivePages;

    [Header("Login Page")]
    [SerializeField] private GameObject icmLoginPage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HideSensitivePages();
        }
    }

    private void HideSensitivePages()
    {
        bool anyHidden = false;

        foreach (GameObject page in sensitivePages)
        {
            if (page != null && page.activeSelf)
            {
                page.SetActive(false);
                anyHidden = true;
                Debug.Log($"[PrivacyManager] Hiding sensitive page: {page.name}");
            }
        }

        if (anyHidden && icmLoginPage != null)
        {
            icmLoginPage.SetActive(true);
            Debug.Log("[PrivacyManager] Redirecting to ICMLoginPage.");
        }
    }
}
