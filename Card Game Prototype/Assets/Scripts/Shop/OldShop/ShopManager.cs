using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public GameObject screenContent;
    public GameObject PackPreab;
    public int packPriceGold;
    public int packPriceGems;
    public Transform packsParent;
    public Transform initialPackSpot;
    public float posXRange = 4f;
    public float posYRange = 8f;
    public float rotationRange = 10f;
    public Text goldText;
    public Text gemsText;
    public Text dustText;
    public GameObject moneyHud;
    public GameObject dustHud;
    public PackOpeningArea OpeningArea;

    public int startingAmountOfGold = 1000;
    public int startingAmountOfGems = 1000;
    public int startingAmountOfDust = 1000;

    public static ShopManager instance;
    public int packsCreated { get; set; }
    private float packPlacementOffset = -0.01f;

    private void Awake()
    {
        instance = this;
        HideScreen();

        if (PlayerPrefs.HasKey("UnopenedPacks"))
        {
            Debug.Log("Unopened Packs: " + PlayerPrefs.GetInt("UnopenedPacks"));
            StartCoroutine(GivePacks(PlayerPrefs.GetInt("UnopenedPacks"), true));
        }

       LoadDustAndMoneyFromPlayerPrefs();
    }

    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            goldText.text = gold.ToString();
        }
    }

    private int gems;
    public int Gems
    {
        get { return gems; }
        set
        {
            gems = value;
            gemsText.text = gems.ToString();
        }
    }

    private int dust;
    public int Dust
    {
        get { return dust; }
        set
        {
            dust = value;
            dustText.text = dust.ToString();
        }
    }

    public void BuyPack(bool usingGold)
    {
        if (usingGold)
        {
            if (gold >= packPriceGold)
            {
                Gold -= packPriceGold;
                StartCoroutine(GivePacks(1));
            }
        }
        else
        {
            if (gems >= packPriceGems)
            {
                Gems -= packPriceGems;
                StartCoroutine(GivePacks(1));
            }
        }
    }

    public IEnumerator GivePacks(int numberOfPacks, bool instant = false)
    {
        for (int i = 0; i < numberOfPacks; i++)
        {
            GameObject newPack = Instantiate(PackPreab, packsParent);
            Vector3 localPositionForNewPack = new Vector3(Random.Range(-posXRange, posXRange), Random.Range(posYRange, posYRange), packsCreated * packPlacementOffset);
            newPack.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-rotationRange, rotationRange));
            packsCreated++;

            //make the last pack appear over the other packs
            newPack.GetComponentInChildren<Canvas>().sortingOrder = packsCreated;
            if (instant)
            {
                newPack.transform.localPosition = localPositionForNewPack;
            }
            else
            {
                newPack.transform.position = initialPackSpot.position;
                newPack.transform.DOLocalMove(localPositionForNewPack, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield break;
    }

    private void OnApplicationQuit()
    {
        SaveDustAndMoneyIntoPlayerPrefs();

        PlayerPrefs.SetInt("UnopenedPacks", packsCreated);
    }
    public void LoadDustAndMoneyFromPlayerPrefs()
    {
        //load Dust
        if (PlayerPrefs.HasKey("Dust"))
        {
            Dust = PlayerPrefs.GetInt("Dust");
        }
        else
        {
            Dust = startingAmountOfDust;
        }
        //load gold
        if (PlayerPrefs.HasKey("Gold"))
        {
            Gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            Gold = startingAmountOfGold;
        }
        //load gems
        if (PlayerPrefs.HasKey("Gems"))
        {
            Gems = PlayerPrefs.GetInt("Gems");
        }
        else
        {
            Gems = startingAmountOfGems;
        }
    }

    public void SaveDustAndMoneyIntoPlayerPrefs()
    {
        PlayerPrefs.SetInt("Dust", dust);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Gems", gems);
    }

    public void ShowScreen()
    {
        screenContent.SetActive(true);
        moneyHud.SetActive(true);
        dustHud.SetActive(true);
    }

    public void HideScreen()
    {
        screenContent.SetActive(false);
        moneyHud.SetActive(false);
        dustHud.SetActive(false);   
    }
}
