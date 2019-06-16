using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity : ScriptableObject
{
    // Defines what the AI should do
    public abstract void Act(StateController controller);
}
