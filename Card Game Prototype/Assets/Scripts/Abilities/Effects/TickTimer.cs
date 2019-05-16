using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTimer : MonoBehaviour
{
    [HideInInspector] public IEnumerator tickTimer;
    [HideInInspector] public TickingEffect targetEffect;

    private int timer;
    int x;

    public void SetTimer (int damage, int frequency, int duration, TickingEffect target)
    {
        targetEffect = target;
        timer = duration;

        if (tickTimer == null)
        {
            tickTimer = ITickTimer(damage, frequency);
            StartCoroutine(tickTimer);
        }
    }

    IEnumerator ITickTimer(int damage, int frequency)
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(frequency);
            // Do effect here
            timer -= frequency;
            targetEffect.TickEffect(this.gameObject);
            ++x;
        }

        tickTimer = null;
        //Debug.Log(this.gameObject + "was triggered " + x + " times");
        Destroy(this);
    }
}
