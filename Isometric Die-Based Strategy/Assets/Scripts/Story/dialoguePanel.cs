using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class dialoguePanel : MonoBehaviour, IPointerClickHandler {
    public Text charName;
    public Text dialogueText;
    RectTransform dialoguePos;
    public InkStory story;
    int pos;

    public void Awake()
    {
        dialoguePos = dialogueText.GetComponent<RectTransform>();
        pos = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData);
        story.RefreshView();   
    }
    public void SetName(string name)
    {
        charName.text = name;
    }

    public void SetText(string text)
    {
        dialogueText.text = text;
    }

    public void CreateChoice(Button choice)
    {
        RectTransform rect = choice.GetComponent<RectTransform>();
        float y = dialoguePos.anchoredPosition.y - dialoguePos.sizeDelta.y/2;
        rect.anchoredPosition = new Vector2(20f, y - 5f - pos*20f);
        ++pos;
    }
}
