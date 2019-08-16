using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    Base_Stats bStats;
    // Controls animations, carries out AI actions (attacking, death, etc) which are driven by the StateController and Audio for player.
    public AudioSource vocalSource;
    public AudioSource effectsSource;
    public AudioSource footStepSource;
    public AudioClip[] attackNoise;
    public AudioClip[] hurtNoise;
    public AudioClip[] deathNoise;
    public AudioClip[] victoryNoise;
    public AudioClip[] defeatNoise;
    public List<AudioClip> weaponAudio;
    public AudioClip footSteps;

    private void Awake()
    {
        vocalSource = GetComponent<AudioSource>();
        bStats = GetComponent<Base_Stats>();
        effectsSource = gameObject.AddComponent<AudioSource>();
        footStepSource = gameObject.AddComponent<AudioSource>();
    }

    public void Attack()
    {
        //Debug.Log("" + this.tag + " is attacking!");
        if (bStats.dead)
            return;

        if (attackNoise.Length > 0)
        {
            int index = Random.Range(0, attackNoise.Length);
            if (attackNoise[index] != null)
            {
                vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                vocalSource.clip = attackNoise[index];
                vocalSource.Play();
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
            int index = Random.Range(0, hurtNoise.Length);
            if (hurtNoise[index] != null)
            {
                vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                vocalSource.clip = hurtNoise[index];
                vocalSource.Play();
            }
        }
    }

    public void Die()
    {
        //Debug.Log("" + this.tag + " diededed!");
        if (deathNoise.Length > 0)
        {
            int index = Random.Range(0, deathNoise.Length);
            if (deathNoise[index] != null)
            {
                vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                vocalSource.clip = deathNoise[index];
                vocalSource.Play();
            }
        }
    }

    public void Ability(AudioClip abilityNoise)
    {
        //Debug.Log("" + this.tag + " was hit by ability!");
        if (abilityNoise != null)
        {
            vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
            vocalSource.clip = abilityNoise;
            vocalSource.Play();
        }
    }

    public void Victory()
    {
        //Debug.Log("(" + this.tag + ") Wins!");
        if (victoryNoise.Length > 0)
        {
            int index = Random.Range(0, victoryNoise.Length);
            if (victoryNoise[index] != null)
            {
                vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                vocalSource.clip = victoryNoise[index];
                vocalSource.Play();
            }
        }
    }

    public void Defeat()
    {
        //Debug.Log("(" + this.tag + ") Loses!");
        if (defeatNoise.Length > 0)
        {
            int index = Random.Range(0, defeatNoise.Length);
            if (defeatNoise[index] != null)
            {
                vocalSource.volume = PlayerPreferances.Vocals * PlayerPreferances.Master;
                vocalSource.clip = defeatNoise[index];
                vocalSource.Play();
            }
        }
    }

    public void WeaponEffects()
    {
        if (weaponAudio.Count > 0)
        {
            int index = Random.Range(0, weaponAudio.Count);
            AudioClip[] audioArray = weaponAudio.ToArray();
            if (audioArray[index] != null)
            {
                effectsSource.volume = PlayerPreferances.SFX * PlayerPreferances.Master;
                effectsSource.clip = audioArray[index];
                effectsSource.Play();
            }
        }
    }

    public void PlayFootSteps()
    {
        if (footSteps == null)
            return;

        footStepSource.volume = PlayerPreferances.SFX * PlayerPreferances.Master;
        footStepSource.clip = footSteps;
        footStepSource.Play();
        footStepSource.loop = true;
    }

    public void StopFootSteps()
    {
        footStepSource.Pause();
    }

    public void AddWeaponAudio(AudioClip audioClip)
    {
        if(!weaponAudio.Contains(audioClip))
            weaponAudio.Add(audioClip);
    }
}
