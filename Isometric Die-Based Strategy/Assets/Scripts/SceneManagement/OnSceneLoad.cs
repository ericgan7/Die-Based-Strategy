using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        InkStory story = FindObjectOfType<InkStory>();
        GameEventManager ge = FindObjectOfType<GameEventManager>();
        ge.load();
        story.LoadStory();
    }
}
