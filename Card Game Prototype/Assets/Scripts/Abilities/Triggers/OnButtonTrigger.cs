using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnButtonTrigger : MonoBehaviour
{
    public OnButton buttonTarget;
    public float coolDown;
    public bool oncePerRound;
    public GameObject target;
    public Image cooldownImage;

    private float timer;
    private bool ready;
    private bool used;

    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        //cooldownImage = this.gameObject.GetComponentInChildren<Image>();
        cooldownImage.enabled = false;
        ready = true;
        timer = 0;
        used = false;
        oncePerRound = buttonTarget.oncePerRound;
        coolDown = buttonTarget.coolDown;
        GetComponent<Image>().sprite = buttonTarget.buttonImage;
    }

    private void Update()
    {
        if (!ready && !used)
        {
            timer -= Time.deltaTime;
            cooldownImage.fillAmount = timer / coolDown;

            if (timer <= 0)
            {
                ready = true;
                timer = 0;
                cooldownImage.enabled = false;
            }
        }
    }

    public void ButtonPressed()
    {
        if (ready)
        {
            buttonTarget.ApplyEffect(target);
            timer = coolDown;
            cooldownImage.enabled = true;
            cooldownImage.fillAmount = 1f;
            ready = false;

            if (oncePerRound)
                used = true;
        }
        else
        {
            // Some kind of feedback to signal it's still on cooldown
        }
    }
}
