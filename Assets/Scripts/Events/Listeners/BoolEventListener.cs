using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class BoolUnityEvent : UnityEvent<bool> { }

[System.Serializable]
public class BoolEventListener : EventListener<bool, BoolEvent, BoolUnityEvent>
{
    
}
