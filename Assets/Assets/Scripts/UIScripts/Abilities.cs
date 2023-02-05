using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public float scaleFactor = 1.2f;
    private Vector3 originalScale;
    public Button button;
    private RectTransform rt;
    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        rt = button.GetComponent<RectTransform>();
        button.onClick.AddListener(() =>
        {
            transform.localScale *= scaleFactor;
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition))
        {
            transform.localScale = originalScale;
        }
    }
}