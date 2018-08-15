using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class InkStory : MonoBehaviour {
    [SerializeField]
    private TextAsset inkJSONAsset;
    private Story story;

    [SerializeField]
    private Canvas canvas;

    // UI Prefabs
    [SerializeField]
    private GameObject dialoguePrefab;
    [SerializeField]
    private Button buttonPrefab;

    private GameManager gm;
    private CharacterManager cm;
    private dialoguePanel dp;
    private GameSceneManager sm;
    private GameState gs;
    private string currentSpeaker;
    public bool battle;
    public bool result;

    void Start() {
        cm = FindObjectOfType<CharacterManager>();
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<GameSceneManager>();
        gs = FindObjectOfType<GameState>();
        RemoveChildren();
        StartStory();
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        story.BindExternalFunction("place_char", (string name, int location, bool speaker) =>
         {
             cm.PlaceChar(name, location);
             if (speaker)
             {
                 currentSpeaker = name;
             }
         });
        string progress = gs.loadStory();
        if (progress != null)
        {
            story.state.LoadJson(progress);
        }
        RefreshView();
    }

    public void RefreshView()
    {
        if (result)
        {
            battle = result; 
        }
        else if (story.canContinue)
        {
            RemoveChildren();
            string text = story.Continue();
            text.Trim();
            CreateContentView(text);

            if (story.currentChoices.Count > 0)
            {
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    Button button = CreateChoiceView(choice.text.Trim());
                    // Tell the button what to do when we press it
                    button.onClick.AddListener(delegate
                    {
                        OnClickChoiceButton(choice);
                    });
                }
            }
            result = parseFunctions();
        }
    }

    void CreateContentView(string text)
    {
        GameObject d = Instantiate(dialoguePrefab);
        dp = d.GetComponent<dialoguePanel>();
        dp.story = this;
        dp.SetText(text);
        dp.SetName(currentSpeaker);
        dp.transform.SetParent(canvas.transform, false);
    }

    // Creates a button showing the choice text
    Button CreateChoiceView(string text)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(dp.transform, false);
        dp.CreateChoice(choice);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;

        return choice;
    }

    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    void RemoveChildren()
    {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            GameObject.Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }
    
    bool parseFunctions()
    {
        for (int i = 0; i < story.currentTags.Count; ++i)
        {
            string[] tag = story.currentTags[i].Split('=');
            if (tag[0] == "function")
            {
                if (tag[1].Contains("startBattle"))
                {
                    string[] parameters = tag[1].Split('(')[1].Split(',');
                    StartCoroutine(startBattle(parameters[0], int.Parse(parameters[1])));
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator startBattle(string sceneName, int severity)
    {
        Debug.Log("START");
        while (battle == false)
        {
            Debug.Log("WAIT");
            yield return new WaitForSeconds(0.5f);
        }
        battle = false;
        Debug.Log("FIGHT");
        gs.saveStory(story.state.ToJson());
        sm.loadScene(sceneName);
    }
}
