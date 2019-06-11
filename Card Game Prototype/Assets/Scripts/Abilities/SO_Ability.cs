using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;

[CreateAssetMenu(menuName = "Card Elements/Abilities/Ability")]
public class SO_Ability : ScriptableObject
{
    public string abName;
    [TextArea]
    public string abDescription;

    public TriggerMethod triggerMethod = TriggerMethod.OnStart;
    public OnDamaged onDamaged;
    public OnHealth onHealth;
    public OnTimer onTimer;
    public OnButton onButton;
    public DeliverySO deliveryMethod;

    public Effect[] effects;

    [HideInInspector] public GameObject item;

    public void LoadAbility (Base_Stats targetPlayer, GameObject gameItem)
    {
        item = gameItem;
        deliveryMethod.ability = this;
        deliveryMethod.owner = targetPlayer.GetComponent<GameObject>();

        switch (triggerMethod)
        {
            case TriggerMethod.OnStart:
                // Apply on Start effect (define what this means! On start of round or On card played?)
                break;
            case TriggerMethod.OnHit:
                targetPlayer.onHitEffectsList.Add(deliveryMethod);
                break;
            case TriggerMethod.OnGetHit:
                targetPlayer.onGetHitEffectsList.Add(deliveryMethod);
                break;
            case TriggerMethod.OnDamaged:
                onDamaged.ability = this;
                targetPlayer.onDamagedList.Add(onDamaged);
                break;
            case TriggerMethod.OnHealth:
                onHealth.ability = this;
                targetPlayer.onHealthList.Add(onHealth);
                break;
            case TriggerMethod.OnTimer:
                onTimer.ability = this;
                targetPlayer.onTimerList.Add(onTimer);
                break;
            case TriggerMethod.OnButton:
                onButton.ability = this;
                // Apply On Button Effect
                break;
            default:
                Debug.Log("Unknown Trigger Method!");
                break;
        }
    }
}
