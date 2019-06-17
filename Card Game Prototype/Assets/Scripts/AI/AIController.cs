using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Controls animations, carries out AI actions (attacking, death, etc) which are driven by the StateController and Audio for player.
    public AudioSource audioSource;
    public AudioClip[] attackNoise;
    public AudioClip[] hurtNoise;
    public AudioClip[] deathNoise;
    public AudioClip[] victoryNoise;
    public AudioClip[] defeatNoise;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        Debug.Log("" + this.tag + " is attacking!");
        int index = Random.Range(0, attackNoise.Length - 1);
        if (attackNoise[index] != null) {
            audioSource.clip = attackNoise[index];
            audioSource.Play();
        }
    }

    public void Hurt ()
    {
        Debug.Log("" + this.tag + " was hurt!");
        int index = Random.Range(0, hurtNoise.Length - 1);
        if (hurtNoise[index] != null)
        {
            audioSource.clip = hurtNoise[index];
            audioSource.Play();
        }
    }

    public void Die()
    {
        Debug.Log("" + this.tag + " diededed!");
        int index = Random.Range(0, deathNoise.Length - 1);
        if (deathNoise[index] != null)
        {
            audioSource.clip = deathNoise[index];
            audioSource.Play();
        }
    }

    public void Ability(AudioClip abilityNoise)
    {
        Debug.Log("" + this.tag + " was hit by ability!");
        if (abilityNoise != null)
        {
            audioSource.clip = abilityNoise;
            audioSource.Play();
        }
    }

    public void Victory()
    {
        Debug.Log("(" + this.tag + ") Wins!");
        int index = Random.Range(0, victoryNoise.Length - 1);
        if (victoryNoise[index] != null)
        {
            audioSource.clip = victoryNoise[index];
            audioSource.Play();
        }
    }

    public void Defeat ()
    {
        Debug.Log("(" + this.tag + ") Loses!");
        int index = Random.Range(0, deathNoise.Length - 1);
        if (deathNoise[index] != null)
        {
            audioSource.clip = deathNoise[index];
            audioSource.Play();
        }
    }
}
