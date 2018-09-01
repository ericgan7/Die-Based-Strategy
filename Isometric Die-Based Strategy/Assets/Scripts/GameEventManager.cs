using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager> {
    private InkStory story;
    /// <summary>
    /// Game Modes available:
    ///     Traveling, where continue forward until next event
    ///     Dialogue, where story takes place
    ///     Battles
    ///     ...
    /// </summary>
    public enum GameMode
    {
        dialogue, travel, explore
    }
    public enum Chapter
    {
        refugee, army, city
    }
    public Chapter type;

    private GameMode mode;
    private GameState gs;
    private MainChar main;
	
	void Start() {
        story = FindObjectOfType<InkStory>();
        mode = GameMode.dialogue;
        main = FindObjectOfType<MainChar>();
        gs = FindObjectOfType<GameState>();
        main.SetAcceleration(2.0f, MainChar.movement.go);
	}

    public void Update()
    {
        if (mode == GameMode.dialogue)
        {

        }
    }

    public void CheckInput()
    {

    }

    public void ContinueTraveling()
    {
        Debug.Log("Continue Traveling");
        main.SetAcceleration(2f, MainChar.movement.go);
        //continue traveling
    }

    public void StartEvent(string eventName)
    {
        if (!gs.CheckEvent(eventName))
        {
            //triggered when party hits a event box
            main.SetAcceleration(0f, MainChar.movement.stop);
            gs.AddEvent(eventName);
            Debug.Log(!gs.CheckEvent(eventName));
            gs.savePosition(type, main.transform.position);
            story.RunEvent(eventName);
        }
    }

    public void load()
    {
        story = FindObjectOfType<InkStory>();
        mode = GameMode.dialogue;
        main = FindObjectOfType<MainChar>();
        Debug.Log(main.maxSpeed);
        Vector3 pos = gs.loadPosition(type);
        if (pos != new Vector3(0f, 0f))
        {
            Debug.Log("Loaded:");
            Debug.Log(pos);
            main.transform.position = pos;
            Debug.Log(main.maxSpeed);
        }
        if (!story.CanContinue())
        {
            Debug.Log(!story.CanContinue());
            ContinueTraveling();
        }
    }
}
