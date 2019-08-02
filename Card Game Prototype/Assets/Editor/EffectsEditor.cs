using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnumTypes;

[CustomEditor(typeof(Effect))]
public class EffectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Effect effect = target as Effect;
        effect.ccType = (CC)EditorGUILayout.EnumPopup(effect.ccType);
        if (effect.ccType != CC.None)
        {
            effect.duration = EditorGUILayout.FloatField(effect.duration);
            if (effect.ccType == CC.Slow || effect.ccType == CC.SlowAttack)
                effect.value = EditorGUILayout.IntField(effect.value);
        }

        effect.statChange = (StatChange)EditorGUILayout.EnumPopup(effect.statChange);
        if (effect.statChange != StatChange.None)
        {
            effect.statChangeValue = EditorGUILayout.IntField(effect.statChangeValue);
            effect.multipler = EditorGUILayout.Toggle(effect.multipler);
            if (effect.statChange == StatChange.MaxHealth)
                effect.changeCurrentHP = EditorGUILayout.Toggle(effect.changeCurrentHP);
        }
    }
}
