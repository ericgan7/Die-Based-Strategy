using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {
    private GameEventManager gm;
    public string eventName;

    void Start()
    {
        gm = FindObjectOfType<GameEventManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Event");
        gm.StartEvent(eventName);
    }
}
