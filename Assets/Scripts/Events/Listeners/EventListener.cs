using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener : MonoBehaviour {
    [SerializeField] protected bool registerOnAwake = false, raiseOnRegister = false;
    protected bool registered = false;

    public virtual void OnEventRaised() {

    }

    public virtual void Register() {
        throw new UnityException("Method not implemented");
    }

    public virtual void Unregister() {
        throw new UnityException("Method not implemented");
    }

    public void Awake() {
        if (registerOnAwake) Register();
    }

    public void OnEnable() {
        Register();
    }

    public void OnDisable() {
        if (!registerOnAwake) Unregister();
    }

    public void OnDestroy() {
        Unregister();
    }
}

public interface IEventListener<T> {
    void OnEventRaised(T first);
}

public interface IEventListener<T1, T2> {
    void OnEventRaised(T1 first, T2 second);
}

[System.Serializable]
public class EventListener<TType, TEvent, TResponse> : EventListener, IEventListener<TType> where TEvent : Event<TType> where TResponse : UnityEvent<TType> {
    [SerializeField] protected TEvent Event;
    [Space]
    [SerializeField] protected TResponse Response;

    public override void OnEventRaised() {
        throw new UnityException("Event raised with no data");
    }

    public virtual void OnEventRaised(TType data) {
        Response.Invoke(data);
    }

    public override void Register() {
        if(!registered) {
            Event.RegisterListener(this, registerOnAwake);
            registered = true;
        }
    }

    public override void Unregister() {
        if(registered) {
            Event.UnregisterListener(this);
            registered = false;
        }
    }
}
