using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTimerManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.EnterCardPhase += StopTimers;
    }

    private void OnDisable()
    {
        GameManager.EnterCardPhase -= StopTimers;
    }

    public void SetTimer (OnTimer TimerTarget, float waitTimer, GameObject intendedTarget)
    {
        StartCoroutine(ITimer(TimerTarget, waitTimer, intendedTarget));
    }

    IEnumerator ITimer(OnTimer TimerTarget, float waitTimer, GameObject intendedTarget)
    {
        yield return new WaitForSeconds(waitTimer);
        TimerTarget.ApplyEffect(intendedTarget);
        //Debug.Log("Timer Triggered!");
        SetTimer(TimerTarget, waitTimer, intendedTarget);
    }

    public void StopTimers ()
    {
        //Debug.Log("Stopping Timer Trigger");
        StopAllCoroutines();
        Destroy(this);
    }
}
