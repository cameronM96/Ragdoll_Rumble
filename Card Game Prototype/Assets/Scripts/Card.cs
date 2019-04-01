using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;

    public Sprite artwork;
    public Sprite background;

    public int damage;
    public  int armour;
    public  int hP;
    public  float speed;
    public  float atkSpeed;

    public string abilityDescription;

    public GameObject[] item;
}
