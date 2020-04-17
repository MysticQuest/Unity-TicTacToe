using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScaleFix : MonoBehaviour
{
    private GridLayoutGroup grid;
    private RectTransform imgRect;
    [SerializeField] float spacing = 5f;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        imgRect = transform.GetComponent<RectTransform>();
        grid.cellSize = new Vector2(imgRect.sizeDelta.x / 3 - spacing, imgRect.sizeDelta.y / 3 - spacing);
    }
}
