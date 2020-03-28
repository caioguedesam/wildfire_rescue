using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class FloatUnityEvent : UnityEvent<float> { }

public class FloatEventListener : EventListener<float, FloatEvent, FloatUnityEvent>
{

}
