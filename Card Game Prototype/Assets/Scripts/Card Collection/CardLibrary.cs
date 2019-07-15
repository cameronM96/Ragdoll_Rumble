using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(menuName = "Card Library")]
public class CardLibrary : ScriptableObject
{
    public List<Card> cardLibrary;

    public Card[] GetCardLibrary ()
    {
        return cardLibrary.ToArray();
    }

    public bool AddCardToLibrary(Card newCard)
    {
        if (cardLibrary.Contains(newCard))
        {
            Debug.Log("Card already exists in library.");
            return false;
        }

        cardLibrary.Add(newCard);

        bool addSuccessful = false;

        addSuccessful = cardLibrary.Contains(newCard);

        if (!addSuccessful)
            Debug.Log("Was unable to add this card to the library");

        return addSuccessful;
    }

    public bool RemoveCardFromLibrary (Card card)
    {
        bool removeSuccessful = false;

        removeSuccessful = cardLibrary.Remove(card);

        if (!removeSuccessful)
            Debug.Log("Could not find this card in library");

        return removeSuccessful;
    }

    public void ClearLibrary()
    {
        cardLibrary.Clear();
    }

    public void LoadLibraryFromFolder(bool newLibrary, string path)
    {
        if (newLibrary)
            cardLibrary.Clear();

        ArrayList al = new ArrayList();
        // Find all folders in path
        string[] folders = Directory.GetDirectories(Application.dataPath + "/" + path + "/");
        if (folders != null)
        {
            foreach (string folder in folders)
            {
                // Make path fit the project folder scheme
                int index = folder.LastIndexOf("/");
                string localPath = path;

                if (index > 0)
                    localPath += folder.Substring(index);

                // Recursively loop through each folder
                LoadLibraryFromFolder(false, localPath);
            }
        }

        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path + "/");

        foreach (string fileName in fileEntries)
        {
            // Make path fit the project folder scheme
            int index = fileName.LastIndexOf("/");
            string localPath = "Assets/" + path;

            if (index > 0)
                localPath += fileName.Substring(index);
            
            // load the object if it is a card
            object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(Card));

            if (t != null)
                al.Add(t);
        }

        // Load cards into card library
        for (int i = 0; i < al.Count; i++)
        {
            cardLibrary.Add((Card)al[i]);
        }

        Debug.Log("Card Library Updated");
    }
}
