using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState> {
    //stores the save stat of variables throught the game
    private string storyProgress = null;


    public void saveStory(string progress)
    {
        storyProgress = progress;
    }
    public string loadStory()
    {
        return storyProgress;
    }
    
    public void changeState(string type, int amount)
    {

    }
}
