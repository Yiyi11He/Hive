using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBar : MonoBehaviour
{
    public Transform topAnchor;
    public Transform bottomAnchor;
    public Transform handle;

    private ScrollTexture scrollTexture;

    private void Awake()
    {
        scrollTexture = GetComponentInParent<ScrollTexture>();
    }

    private void Update()
    {
        handle.position = Vector3.Lerp(bottomAnchor.position, topAnchor.position, scrollTexture.normalisedScrollProgress);
    }
}
