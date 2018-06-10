using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDisplay : MonoBehaviour {

    public GameObject attackPanel;
    public GameObject defensePanel;
    private List<pair> attackDie;
    private List<pair> defenseDie;

    public int attackIndex;
    public int defenseIndex;
    public float dieWidth;
    public float dieHeight;
    public int incrementDirection;

    public void Start()
    {
        attackIndex = -1;
        defenseIndex = -1;
        attackDie = new List<pair>();
        defenseDie = new List<pair>();
    }

    public Vector3 addDie(GameObject die, int value, dieMovement.dieType type)
    {
        switch (type)
        {
            case dieMovement.dieType.regular:
                switch (value)
                {
                    case 1: //attack
                    case 2:
                    case 3:
                        return addAttack(value, die);
                    case 4: //defense
                    case 5:
                    case 6:
                        return addDefense(value, die);
                }
                break;
            case dieMovement.dieType.pierce:
            default:
                    switch (value) {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        return addAttack(value, die);
                    case 5:
                    case 6:
                        return addDefense(value, die);
                    }
                break;
        }
        return Vector3.zero;
    }

    Vector3 addAttack(float value, GameObject die)
    {
        ++attackIndex;
        attackDie.Add(new pair(value, die));
        return attackPanel.transform.TransformPoint(new Vector3(attackPanel.transform.localPosition.x + incrementDirection * attackIndex % 7 * dieWidth,
            attackPanel.transform.localPosition.y + Mathf.Floor(attackIndex / 7) * dieHeight,
            0f));
    }

    Vector3 addDefense(float value, GameObject die)
    {
        ++defenseIndex;
        defenseDie.Add(new pair(value, die));
        return defensePanel.transform.TransformPoint(new Vector3(defensePanel.transform.localPosition.x + incrementDirection * defenseIndex % 7 * dieWidth,
            defensePanel.transform.localPosition.y + Mathf.Floor(defenseIndex / 7) * dieHeight,
            0f));
    }

    public void reset()
    {

    }

    public void removeDie(bool attack)
    {
        //add animation?
        if (attack)
        {
            attackDie[attackIndex].value.SetActive(false);
            --attackIndex;
        }
        else
        {
            defenseDie[defenseIndex].value.SetActive(false);
            --defenseIndex;
        }
    }
}   
