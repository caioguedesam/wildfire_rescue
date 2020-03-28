using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class Vector3UnityEvent : UnityEvent<Vector3> { }

public class Vector3EventListener : EventListener<Vector3, Vector3Event, Vector3UnityEvent>
{
    
}
