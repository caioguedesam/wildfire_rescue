using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class GOUnityEvent : UnityEvent<GameObject> { }

public class GOEventListener : EventListener<GameObject, GOEvent, GOUnityEvent>
{
    
}
