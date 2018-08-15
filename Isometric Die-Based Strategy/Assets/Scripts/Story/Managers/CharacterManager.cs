using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager> {
    public GameObject[] characters;
    public Dictionary<string,Character> charData;
    public List<Character> activeChar;
    public GameObject[] location;

	void Awake () {
        activeChar = new List<Character>();
        charData = new Dictionary<string, Character>();
        for (int i = 0; i < characters.Length; ++i)
        {
            GameObject c = Instantiate(characters[i]);
            c.name = characters[i].name;
            charData[c.name] = c.GetComponent<Character>();
            Debug.Log(c.name);
        }
	}

    public void PlaceChar(string actorName, int l)
    {
        activeChar.Clear();
        Character c = charData[actorName];
        activeChar.Add(c);
        c.gameObject.SetActive(true);
        c.transform.position = location[l].transform.position;
    }
	
	void changeState(Character.characterEmotion state)
    {
        switch (state)
        {
            case Character.characterEmotion.neutral:
            default:
                break;
        }
    }
}
