using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Base for all events
public abstract class Event : ScriptableObject {
    [Multiline][SerializeField] private string description = "";

    public virtual void Raise() {
        throw new UnityException("Method not implemented");
    }

    public virtual void RegisterListener(EventListener listener, bool raiseOnRegister = false) {
        throw new UnityException("Method not implemented");
    }

    public virtual void UnregisterListener(EventListener listener) {
        throw new UnityException("Method not implemented");
    }
}

// Template class for events with values
[System.Serializable]
public abstract class Event<T> : Event {
    private List<IEventListener<T>> listeners = new List<IEventListener<T>>();

    public override void Raise() {
        Debug.Log("Raised " + this.name + " event");
    }

    public virtual void Raise(T data) {
        for (int i = listeners.Count - 1; i >= 0; i--) {
            listeners[i].OnEventRaised(data);
        }
    }

    public virtual void RegisterListener(IEventListener<T> listener, bool raiseOnRegister = false) {
        listeners.Add(listener);
        if (raiseOnRegister) Raise();
    }

    public virtual void UnregisterListener(IEventListener<T> listener) {
        listeners.Remove(listener);
    }
}