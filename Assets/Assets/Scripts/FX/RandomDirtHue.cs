using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirtHue : MonoBehaviour
{
    private static readonly int Hue = Shader.PropertyToID("_Hue");
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _hueRange = 10f;

    void Awake()
    {
        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat(Hue, Random.Range(-_hueRange, _hueRange));
        _spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}
