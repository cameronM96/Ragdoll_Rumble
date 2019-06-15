using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTimer : MonoBehaviour
{
    [HideInInspector] public IEnumerator tickTimer;
    [HideInInspector] public TickingEffect targetEffect;

    private int timer;

    private void OnEnable()
    {
        GameManager.EnterCardPhase += StopTimers;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= StopTimers;
    }

    public void SetTimer (int damage, int frequency, int duration, TickingEffect target)
    {
        targetEffect = target;
        timer = duration;

        if (tickTimer == null)
        {
            if (targetEffect.applyTickOnStart)
                targetEffect.TickEffect(this.gameObject);

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
        }

        if (targetEffect.applyTickOnEnd)
            targetEffect.TickEffect(this.gameObject);

        tickTimer = null;
        Destroy(this);
    }

    public void StopTimers()
    {
        //Debug.Log("Stopping Ticking Effect");
        StopAllCoroutines();
        Destroy(this);
    }
}
