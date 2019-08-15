﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Base_Stats bStats;
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
        bStats = GetComponent<Base_Stats>();
    }

    public void Attack()
    {
        //Debug.Log("" + this.tag + " is attacking!");
        if (bStats.dead)
            return;

        if (attackNoise.Length > 0)
        {
            int index = Random.Range(0, attackNoise.Length - 1);
            if (attackNoise[index] != null)
            {
                audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                audioSource.clip = attackNoise[index];
                audioSource.Play();
            }
        }
    }

    public void Hurt ()
    {
        if (bStats.dead)
            return;
        //Debug.Log("" + this.tag + " was hurt!");
        if (hurtNoise.Length > 0)
        {
            int index = Random.Range(0, hurtNoise.Length - 1);
            if (hurtNoise[index] != null)
            {
                audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                audioSource.clip = hurtNoise[index];
                audioSource.Play();
            }
        }
    }

    public void Die()
    {
        //Debug.Log("" + this.tag + " diededed!");
        if (deathNoise.Length > 0)
        {
            int index = Random.Range(0, deathNoise.Length - 1);
            if (deathNoise[index] != null)
            {
                audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                audioSource.clip = deathNoise[index];
                audioSource.Play();
            }
        }
    }

    public void Ability(AudioClip abilityNoise)
    {
        //Debug.Log("" + this.tag + " was hit by ability!");
        if (abilityNoise != null)
        {
            audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            audioSource.clip = abilityNoise;
            audioSource.Play();
        }
    }

    public void Victory()
    {
        //Debug.Log("(" + this.tag + ") Wins!");
        if (victoryNoise.Length > 0)
        {
            int index = Random.Range(0, victoryNoise.Length - 1);
            if (victoryNoise[index] != null)
            {
                audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                audioSource.clip = victoryNoise[index];
                audioSource.Play();
            }
        }
    }

    public void Defeat ()
    {
        //Debug.Log("(" + this.tag + ") Loses!");
        if (defeatNoise.Length > 0)
        {
            int index = Random.Range(0, defeatNoise.Length - 1);
            if (defeatNoise[index] != null)
            {
                audioSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                audioSource.clip = defeatNoise[index];
                audioSource.Play();
            }
        }
    }
}
