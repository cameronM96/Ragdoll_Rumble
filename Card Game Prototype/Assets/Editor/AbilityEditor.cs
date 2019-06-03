using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnumTypes;

[CustomEditor(typeof(SO_Ability))]
public class AbilityEditor : Editor
{
    SerializedProperty effects;

    private void OnEnable()
    {
        effects = serializedObject.FindProperty("effects");
    }

    public override void OnInspectorGUI()
    {
        SO_Ability ability = target as SO_Ability;

        ability.abName = EditorGUILayout.TextField("Ability Name:", ability.abName);

        EditorGUILayout.LabelField("Ability Description:");
        ability.abDescription = EditorGUILayout.TextArea(ability.abDescription,GUILayout.Height(100));

        ability.triggerMethod = (TriggerMethod)EditorGUILayout.EnumPopup(ability.triggerMethod);

        switch (ability.triggerMethod)
        {
            case TriggerMethod.OnStart:
                break;
            case TriggerMethod.OnHit:
                break;
            case TriggerMethod.OnGetHit:
                break;
            case TriggerMethod.OnDamaged:
                ability.onDamaged = EditorGUILayout.ObjectField("OnDamaged Trigger:", ability.onDamaged, typeof(OnDamaged), false) as OnDamaged;
                break;
            case TriggerMethod.OnHealth:
                ability.onHealth = EditorGUILayout.ObjectField("OnHealth Trigger:", ability.onHealth, typeof(OnHealth), false) as OnHealth;
                break;
            case TriggerMethod.OnTimer:
                ability.onTimer = EditorGUILayout.ObjectField("OnTimer Trigger:", ability.onTimer, typeof(OnTimer), false) as OnTimer;
                break;
            case TriggerMethod.OnButton:
                ability.onButton = EditorGUILayout.ObjectField("OnButton Trigger:", ability.onButton, typeof(OnButton), false) as OnButton;
                break;
            default:
                break;
        }

        ability.deliveryMethod = EditorGUILayout.ObjectField("Delivery Method:", ability.deliveryMethod, typeof(DeliverySO), false) as DeliverySO;

        SerializedObject so = new SerializedObject(target);
        SerializedProperty effectsProperty = so.FindProperty("effects");

        EditorGUILayout.PropertyField(effectsProperty, includeChildren: true);

        so.ApplyModifiedProperties();
    }
}
