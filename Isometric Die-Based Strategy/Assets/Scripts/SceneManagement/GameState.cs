using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState> {
    //stores the save stat of variables throught the game
    private string storyProgress;
    //keeps track of what characters are alive, stats, and items.
    private Vector3 refugeePosition;
    private List<Character> refugee;
    private List<Character> army;
    private List<Character> city;

    private Dictionary<string, int> events;

    void Start()
    {
        storyProgress = null;
        refugeePosition = new Vector3(0f, 0f);
        events = new Dictionary<string, int>();
    }

    public void saveStory(string progress)
    {
        storyProgress = progress;
    }

    public string loadStory()
    {
        return storyProgress;
    }

    public void AddEvent(string eventName)
    {
        events.Add(eventName, 0);
    }

    public bool CheckEvent(string eventName)
    {
        return events.ContainsKey(eventName);
    }

    public void savePosition(GameEventManager.Chapter type, Vector3 position)
    {
        if (type == GameEventManager.Chapter.refugee)
        {
            refugeePosition = position;
        }
    }

    public Vector3 loadPosition(GameEventManager.Chapter type)
    {
        if (type == GameEventManager.Chapter.refugee)
        {
            return refugeePosition;
        }
        else
        {
            return new Vector3(0f, 0f);
        }
    }
    
    public void updateSupplies(int supply) {

    }

    public void updateMorale(int morale)
    {

    }
    public void updateItem(string item)
    {

    }

    public void updateGold(int gold)
    {

    }
}
