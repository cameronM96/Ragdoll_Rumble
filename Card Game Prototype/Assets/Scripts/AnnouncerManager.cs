using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] battleBegin;
    public AudioClip[] round;
    public AudioClip[] roundConculsion;
    public AudioClip[] victory;
    public AudioClip[] defeat;
    public AudioClip[] tie;
    public AudioClip[] countDown;

    public float audioClipOffset = 1f;

    public void Round(int roundNumb)
    {
        if (round[roundNumb] != null)
        {
            audioSource.clip = round[roundNumb];
            audioSource.Play();
        }

        StartCoroutine(WaitforAudioClip(audioSource.clip.length, audioClipOffset));
    }
    IEnumerator WaitforAudioClip(float clipLength, float waitOffset)
    {
        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(clipLength + waitOffset);
        }

        BeginFight();
    }

    public void BeginFight ()
    {

        int index = Random.Range(0, battleBegin.Length - 1);
        if (battleBegin[index] != null)
        {
            audioSource.clip = battleBegin[index];
            audioSource.Play();
        }
    }

    public void RoundConclusion (string winningTeam)
    {
        int index = Random.Range(0, roundConculsion.Length - 1);
        if (roundConculsion[index] != null)
        {
            audioSource.clip = roundConculsion[index];
            audioSource.Play();
        }
    }

    public void Victory ()
    {
        int index = Random.Range(0, victory.Length - 1);
        if (victory[index] != null)
        {
            audioSource.clip = victory[index];
            audioSource.Play();
        }
    }

    public void Defeat ()
    {
        int index = Random.Range(0, defeat.Length - 1);
        if (defeat[index] != null)
        {
            audioSource.clip = defeat[index];
            audioSource.Play();
        }
    }

    public void Tie ()
    {
        int index = Random.Range(0, tie.Length - 1);
        if (tie[index] != null)
        {
            audioSource.clip = tie[index];
            audioSource.Play();
        }
    }

    public void CountDown (int seconds)
    {
        if (seconds == 30)
        {
            // 30 second warning
            if (countDown[6] != null)
            {
                audioSource.clip = countDown[6];
                audioSource.Play();
            }
        }
        else if (seconds == 10)
        {
            // 10 second warning
            if (countDown[5] != null)
            {
                audioSource.clip = countDown[6];
                audioSource.Play();
            }
        }
        else if (seconds <= 5 && seconds > 0)
        {
            // 5,4,3,2,1 count down
            if (countDown[seconds - 1] != null)
            {
                audioSource.clip = countDown[seconds - 1];
                audioSource.Play();
            }
        }
    }
}
