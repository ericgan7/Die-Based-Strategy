using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIPanel : MonoBehaviour {
    public Vector2 defaultPosition;
    private RectTransform rect;
    public int direction;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 2);
        rect.anchoredPosition = new Vector3(direction * Screen.width / 2, transform.position.y);
        defaultPosition = rect.anchoredPosition;
    }
}
