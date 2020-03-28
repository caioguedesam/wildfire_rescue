using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener : EventListener {
    [SerializeField] protected BaseEvent Event;
    [Space]
    [SerializeField] protected UnityEvent Response;

    public override void OnEventRaised() {
        Response.Invoke();
    }

    public override void Register() {
        if (!registered) {
            Event.RegisterListener(this, raiseOnRegister);
            registered = true;
        }
    }

    public override void Unregister() {
        if (registered) {
            Event.UnregisterListener(this);
            registered = false;
        }
    }
}
