using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Packs Holder")]
public class PacksHolder : ScriptableObject
{
    public List<PackOfCards> packs;

    [HideInInspector] public string storedPath;
    [HideInInspector] public bool newLibrary;

    public bool AddCardToLibrary(PackOfCards newPack)
    {
        if (packs.Contains(newPack))
        {
            Debug.Log("Card already exists in library.");
            return false;
        }

        packs.Add(newPack);

        bool addSuccessful = false;

        addSuccessful = packs.Contains(newPack);

        if (!addSuccessful)
            Debug.Log("Was unable to add this card to the library");

        return addSuccessful;
    }

    public bool RemoveCardFromLibrary(PackOfCards newPack)
    {
        bool removeSuccessful = false;

        removeSuccessful = packs.Remove(newPack);

        if (!removeSuccessful)
            Debug.Log("Could not find this card in library");

        return removeSuccessful;
    }

    public void ClearLibrary()
    {
        packs.Clear();
    }
}
