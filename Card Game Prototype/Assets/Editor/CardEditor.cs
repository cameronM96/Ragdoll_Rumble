using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EnumTypes;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Card card = target as Card;

        card.currentCardType = (CardType)EditorGUILayout.EnumPopup("Card Type: ", card.currentCardType);
        card.playableSlots = (PlayableSlot)EditorGUILayout.EnumFlagsField("PlayableSlots: ", card.playableSlots);

        base.OnInspectorGUI();
    }
}
