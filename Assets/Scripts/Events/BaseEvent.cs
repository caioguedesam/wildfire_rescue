using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regular event (without values associated)
[CreateAssetMenu(fileName = "Base_Event_Name", menuName = "Events/New Event", order = 1)]
public class BaseEvent : Event {
    private List<EventListener> listeners = new List<EventListener>();

    public void OnEnable() {
        listeners = new List<EventListener>();
    }

    public override void Raise() {
        Debug.Log("Raised " + this.name + ", listeners: " + listeners.Count);

        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnEventRaised();
        }
    }

    public override void RegisterListener(EventListener listener, bool raiseOnRegister = false) {
        listeners.Add(listener);
        if (raiseOnRegister) Raise();
    }

    public override void UnregisterListener(EventListener listener) {
        listeners.Remove(listener);
    }
}
