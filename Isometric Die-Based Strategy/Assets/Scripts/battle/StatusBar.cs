using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public Image hp;
    public Image end;
    public GameObject targetPos;
    private RectTransform hpPos;
    private RectTransform endPos;

    void Awake()
    {
        hpPos = hp.GetComponent<RectTransform>();
        endPos = end.GetComponent<RectTransform>();
    }

    public void updateHealth(int current, int max)
    {
        hpPos.anchoredPosition = new Vector2(0f, (current - max) * 2f / max);
    }

    public void updateEnd(int current, int max)
    {
        endPos.anchoredPosition = new Vector2(0f, (current - max) * 2f / max);
    }

    public void Update()
    {
        transform.position = targetPos.transform.position;
    }
}
