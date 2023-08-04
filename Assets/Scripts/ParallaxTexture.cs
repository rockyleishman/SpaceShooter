using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTexture : MonoBehaviour
{
    [SerializeField][Range(1.0f, 20.0f)] public float LayerMultiplier = 1.0f;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void SetOffset(Vector2 offset)
    {
        _material.mainTextureOffset = offset / LayerMultiplier / transform.localScale.x;
    }
}
