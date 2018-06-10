using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler{
    public dieMovement.dieType type;
    public int numDie;
    public int[] statEffects;
    public Game game;

    private RectTransform rect;
    private Quaternion defaultRotation;
    private Vector2 defaultPosition;
    public int place;

    public void Start()
    {
        statEffects = new int[5];
        for (int i = 0; i < 5; ++i)
        {
            statEffects[i] = 0;
        }
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void setPosition(int p)
    {
        defaultRotation = transform.localRotation;
        defaultPosition = rect.anchoredPosition;
        place = p;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(move(rect.anchoredPosition.x, 460.0f, Quaternion.identity));
        rect.localScale = new Vector3(0.2f, 0.8f);
        transform.SetAsLastSibling();
        game.battleUI.moveCards(this, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(0.1f, 0.4f);
        StartCoroutine(move(defaultPosition.x, defaultPosition.y, defaultRotation));
        transform.SetSiblingIndex(place);
        game.battleUI.moveCards(this, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!game.battleUI.rolled)
        {
            game.battleUI.ResetDie();
            game.battleUI.SetDie(numDie, type);
        }
    }
    public void setType(CharacterMovement.charAttacks attackType)
    {
        switch(attackType)
        {
            //added title, image, and description
            case CharacterMovement.charAttacks.normal:
                type = dieMovement.dieType.regular;
                numDie = 0;
                for (int i = 0; i < 5; ++i)
                {
                    statEffects[i] = 0;
                }
                break;
            case CharacterMovement.charAttacks.pierce:
                type = dieMovement.dieType.pierce;
                numDie = 0;
                for (int i = 0; i < 5; ++i)
                {
                    statEffects[i] = 0;
                }
                break;
            case CharacterMovement.charAttacks.flurry:
                type = dieMovement.dieType.flurry;
                numDie = 2;
                for (int i =0; i < 5; ++i)
                {
                    statEffects[i] = 0;
                }
                break;
        }
    }

    public IEnumerator move(float x, float y, Quaternion rotation)
    {
        transform.localRotation = rotation;
        float t = 0.0f;
        float newX;
        float newY;
        while (Mathf.Abs(rect.anchoredPosition.x - x) > 0.01f || Mathf.Abs(rect.anchoredPosition.y - y) > 0.01f)
        {
            t += Time.deltaTime;
            newX = Animations.EaseOutElastic(rect.anchoredPosition.x, x, t);
            newY = Animations.EaseOutElastic(rect.anchoredPosition.y, y, t);
            rect.anchoredPosition = new Vector2(newX, newY);
            yield return new WaitForFixedUpdate();
        }
    }

    public void setType(CharacterMovement.charDefenses type)
    {

    }
}
