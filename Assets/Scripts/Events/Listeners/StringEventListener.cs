using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class StringUnityEvent : UnityEvent<string> { }

public class StringEventListener : EventListener<string, StringEvent, StringUnityEvent>
{
    
}
