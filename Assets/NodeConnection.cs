using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeConnection : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float scrollSpeed = 1.0f;

    private Material lineMaterial;
    private float textureOffset = 0.0f;

    void Start()
    {
        lineMaterial = lineRenderer.material;
    }

    void Update()
    {
        textureOffset += Time.deltaTime * scrollSpeed;
        textureOffset %= 1.0f; // Reset the offset after it reaches 1.0
        //lineMaterial.SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
        lineRenderer.materials[0].SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
    }
}
