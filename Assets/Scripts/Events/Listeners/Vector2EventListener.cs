using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class Vector2UnityEvent : UnityEvent<Vector2> { }

public class Vector2EventListener : EventListener<Vector2, Vector2Event, Vector2UnityEvent>
{
    
}
