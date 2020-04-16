using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScaleFix : MonoBehaviour
{
    public GridLayoutGroup grid;
    public RectTransform imgRect;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        imgRect = transform.GetComponent<RectTransform>();
        grid.cellSize = new Vector2(imgRect.sizeDelta.x / 3, imgRect.sizeDelta.y / 3);
    }
}
