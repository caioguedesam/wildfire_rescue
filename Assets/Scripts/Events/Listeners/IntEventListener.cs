using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class IntUnityEvent : UnityEvent<int> { }

public class IntEventListener : EventListener<int, IntEvent, IntUnityEvent>
{
    
}
