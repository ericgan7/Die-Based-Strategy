using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUI : MonoBehaviour
{

    public Game game;

    public float minForce;
    public float maxForce;
  
    public GameObject leftSpawn;
    private List<GameObject> leftDice;
    public GameObject rightSpawn;
    private List<GameObject> rightDice;

    private Stack<GameObject> blueDie;
    private Stack<GameObject> redDie;
    public GameObject[] blueDieObjects;
    public GameObject[] redDieObjects;

    public GameObject leftPanel;
    public GameObject rightPanel;
    private CharacterMovement playerChar;
    private CharacterMovement oppChar;

    public GameObject[] cards;
    public List<GameObject> activeCards;
    public float cardCurve;
    public float cardSpacing;
    public float cardDrop;

    public bool isThrowing;
    private bool isFighting;
    public bool rolled;
    private bool isLeftAttacker;

    public void Start()
    {
        isFighting = false;
        rolled = false;
        blueDie = new Stack<GameObject>();
        redDie = new Stack<GameObject>();
        leftDice = new List<GameObject>();
        rightDice = new List<GameObject>();
        activeCards = new List<GameObject>();
        for (int i = 0; i < blueDieObjects.Length; ++i)
        {
            blueDie.Push(blueDieObjects[i].gameObject);
        }
        for (int i = 0; i < redDieObjects.Length; ++i)
        {
            redDie.Push(redDieObjects[i].gameObject);
        }
    }

    public void SetBattle(CharacterMovement attacker, CharacterMovement defender, bool isAttacking)
    {
        if (isFighting)
        {
            for (int i = 0; i < leftDice.Count; ++i)
            {
                leftDice[i].SetActive(false);
                blueDie.Push(leftDice[i]);
            }
            for (int i = 0; i < rightDice.Count; ++i)
            {
                rightDice[i].SetActive(false);
                redDie.Push(rightDice[i]);
            }
            for (int i = 0; i < activeCards.Count; ++i)
            {
                activeCards[i].SetActive(false);
            }
            leftDice = new List<GameObject>();
            rightDice = new List<GameObject>();
            activeCards = new List<GameObject>();
            rolled = false;
            SetUI(false);
        }
        else
        {
            isLeftAttacker = isAttacking;
            if (isAttacking) {
                playerChar = attacker;
                oppChar = defender;    
            }
            else
            {
                playerChar = defender;
                oppChar = attacker;
            }
            SetUI(true);
        }
        isFighting = !isFighting;
    }

    public void setCards()
    {
        if (isLeftAttacker)
        {
            int start = -playerChar.attackMoves.Length/2;
            for (int i = 0; i < playerChar.attackMoves.Length; ++i)
            {
                float startX = 0.0f;
                float startY = 365.0f;
                activeCards.Add(cards[i]);
                cards[i].SetActive(true);
                cards[i].GetComponent<RectTransform>().anchoredPosition = (new Vector2(startX + (start+i) * cardSpacing, startY + -Mathf.Abs(start+i) * cardDrop));
                cards[i].transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -(start+i) * cardCurve));
                Card c = cards[i].GetComponent<Card>();
                c.setType(playerChar.attackMoves[i]);
                c.setPosition(i);
            }
        }
        else
        {

        }
    }

    public void resetCards()
    {
        for (int i = 0; i < cards.Length; ++i)
        {
            cards[i].SetActive(false);
        }
    }

    public void moveCards(Card card, bool zoomIn)
    {
        Card c;
        if (zoomIn)
        {
            for (int i = 0; i < activeCards.Count; ++i)
            {
                if (i == card.place)
                {
                    continue;
                }
                c = activeCards[i].GetComponent<Card>();
                if (i < card.place)
                {
                    StartCoroutine(c.move(c.defaultPosition.x - 35.0f,
                       c.defaultPosition.y, c.defaultRotation));
                }
                else if (i > card.place)
                {
                    StartCoroutine(c.move(c.defaultPosition.x + 35.0f,
                       c.defaultPosition.y, c.defaultRotation));
                }
            }
        }
        else
        {
            for (int i = 0; i < activeCards.Count; ++i)
            {
                if (i == card.place)
                {
                    continue;
                }
                c = activeCards[i].GetComponent<Card>();
                StartCoroutine(c.move(c.defaultPosition.x,
                      c.defaultPosition.y, c.defaultRotation));
            }
        }
    }

    public void SetUI(bool start)
    {
        if (start)
        {
            StartCoroutine(StartPanels(leftPanel, 2.0f));
            StartCoroutine(StartPanels(rightPanel, 2.0f));
            setCards();
        }
        else
        {
            BattleUIPanel left = leftPanel.GetComponent<BattleUIPanel>();
            BattleUIPanel right = rightPanel.GetComponent<BattleUIPanel>();
            StartCoroutine(EndPanels(leftPanel, 2.0f, left.defaultPosition.x));
            StartCoroutine(EndPanels(rightPanel, 2.0f, right.defaultPosition.x));
        }
    }

    IEnumerator StartPanels(GameObject panel, float time)
    {
        float t = 0.0f;
        float x;
        bool shake = true;
        RectTransform p = panel.GetComponent<RectTransform>();
        while (t < time)
        {
            t += Time.deltaTime;
            if (shake && Mathf.Abs(p.anchoredPosition.x) < 50.0f)
            {
                shake = false;
                StartCoroutine(Animations.ScreenShake(0.1f, 0.3f));
            }
            x = Animations.EaseOutElastic(p.anchoredPosition.x,0.0f, Time.deltaTime * time);
            p.anchoredPosition = new Vector3(x, 0.0f, 0.0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator EndPanels(GameObject panel, float time, float endPoint)
    {
        float t = 0.0f;
        float x;
        RectTransform p = panel.GetComponent<RectTransform>();
        while (Mathf.Abs(p.anchoredPosition.x - endPoint) > 0.01f)
        {
            t += Time.deltaTime;
            x = Animations.EaseOutElastic(p.anchoredPosition.x, endPoint, Time.deltaTime * time);
            p.anchoredPosition = new Vector3(x, 0.0f, 0.0f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public void SetDie(int num, dieMovement.dieType t)
    {
        if (isFighting)
        {
            Debug.Log("set die");
            int left = 0;
            int right = 0;
            if (isLeftAttacker)
            {
                left = num + playerChar.attack;
                right = oppChar.defense;
            }
            else
            {
                left = num + playerChar.defense;
                right = oppChar.attack;
            }
            Debug.Log(new Vector2(left, right));
            for (int i = 0; i < left; ++i)
            {
                leftDice.Add(blueDie.Pop());
                leftDice[i].transform.rotation = leftSpawn.transform.rotation;
                leftDice[i].GetComponent<dieMovement>().type = t;
                leftDice[i].transform.localPosition = new Vector3(Mathf.Floor(i / 2) * 0.55f, (i % 2) * 0.55f, 0.0f);
                leftDice[i].SetActive(true);
            }


            for (int i = 0; i < right; ++i)
            {
                rightDice.Add(redDie.Pop());
                rightDice[i].transform.rotation = rightSpawn.transform.rotation;
                rightDice[i].GetComponent<dieMovement>().type = dieMovement.dieType.regular;
                rightDice[i].transform.localPosition = new Vector3(Mathf.Floor(i / 2) * 0.55f, (i % 2) * 0.55f, 0.0f);
                rightDice[i].SetActive(true);
            }
            //disable cards
        }
    }

    public void ResetDie()
    {
        for (int i = 0; i < leftDice.Count; ++i)
        {
            blueDie.Push(leftDice[i]);
            leftDice[i].SetActive(false);
        }
        leftDice.Clear();
        for (int i = 0; i < rightDice.Count; ++i)
        {
            redDie.Push(rightDice[i]);
            rightDice[i].SetActive(false);
        }
        rightDice.Clear();
    }

    public void setThrowing()
    {
        if (!rolled)
        {
            isThrowing = true;
        }
    }

    public IEnumerator RollDie(Vector3 position)
    {
        if (isFighting && isThrowing)
        {
            resetCards();
            Vector3 direction;
            float force;
            direction = position - leftSpawn.transform.position;
            force = Vector3.Distance(leftSpawn.transform.position, position);
            for (int i = 0; i < leftDice.Count; ++i)
            {
                leftDice[i].GetComponent<dieMovement>().rollDie(direction, force);
            }
            isThrowing = false;
            rolled = true;
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < rightDice.Count; ++i)
            {
                rightDice[i].GetComponent<dieMovement>().rollDie(new Vector3(-5f, 6.5f, -3f), force);
            }
        }
    }

    public IEnumerator Fight()
    {
        bool start = false; ;
        Debug.Log("rolling Dice before fight");
        int t = 0;
        while (!start) {    //check and ensure all dice are finished rolling
            start = true;
            ++t;
            if (t > 20)
            {
                break;
            }
            for (int i = 0; i < leftDice.Count; ++i)
            {
                dieMovement die = leftDice[i].GetComponent<dieMovement>();
                if (!die.ready)
                {
                    start = false;
                    break;
                }
            }
            if (start)
            {
                for (int i = 0; i < rightDice.Count; ++i)
                {
                    dieMovement die = rightDice[i].GetComponent<dieMovement>();
                    if (!die.ready)
                    {
                        start = false;
                        break;
                    }
                }
            }
            if (!start)
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
        Debug.Log("Begin Fighting");
        StartCoroutine(Attack(isLeftAttacker));
    }

    public IEnumerator Attack(bool isLeft)
    {
        if (isLeft)
        {
            while (game.leftUI.attackIndex >= 0)
            {
                game.leftUI.removeDie(true);
                if (game.rightUI.defenseIndex < 0)
                {
                    --oppChar.health;
                }
                else
                {
                    game.rightUI.removeDie(false);
                }
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1.0f);
            while (game.rightUI.attackIndex >= 0)
            {
                game.rightUI.removeDie(true);
                if (game.leftUI.defenseIndex < 0)
                {
                    --playerChar.health;
                }
                else
                {
                    game.leftUI.removeDie(false);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {

        }
        game.gameController.EndBattle();
        SetUI(false);
    }
}
