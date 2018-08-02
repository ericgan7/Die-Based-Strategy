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

    void Start() {
        cm = GetComponent<CharacterManager>();
        gm = GetComponent<GameManager>();
        RemoveChildren();
        StartStory();
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        story.BindExternalFunction("place_Char",(string name, int location) =>
        {
            cm.PlaceChar(name, location);
        });
        RefreshView();
    }

    public void RefreshView()
    {
        if (story.canContinue)
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
        }
    }

    void CreateContentView(string text)
    {
        GameObject d = Instantiate(dialoguePrefab);
        dp = d.GetComponent<dialoguePanel>();
        dp.story = this;
        dp.SetText(text);
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
}
