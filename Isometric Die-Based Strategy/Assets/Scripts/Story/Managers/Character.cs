using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum characterEmotion
    {
        neutral
    }
    private int injury;
    private int attack;
    private int defense;
    private int health;
    private int endurance;
    private int armor;

    private Item[] equips;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
