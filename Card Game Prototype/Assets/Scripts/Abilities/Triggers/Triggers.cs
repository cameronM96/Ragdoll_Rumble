using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public string triggerName;
    public delegate void EventTrigger();
    public event EventTrigger TriggeredEvent;

    public void Initialise()
    {
        TriggeredEvent?.Invoke();
    }
}
