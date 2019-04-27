using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public Sprite aSprite;
    public AudioClip aSound;

    public string aDescription = "What does this ability do?";

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}
