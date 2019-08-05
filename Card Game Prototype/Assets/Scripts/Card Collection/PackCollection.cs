using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackCollection : MonoBehaviour
{
    public PacksHolder packHolder;

    public Dictionary<int, PackOfCards> packIDDictionary = new Dictionary<int, PackOfCards>();

    private void Start()
    {
        LoadPacks();
    }

    public void LoadPacks()
    {
        foreach (PackOfCards pack in packHolder.packs)
        {
            if(!packIDDictionary.ContainsKey(pack.iD))
            {
                packIDDictionary.Add(pack.iD, pack);
            }
        }
    }
}
