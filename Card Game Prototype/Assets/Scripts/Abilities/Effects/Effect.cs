using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public bool lastForWholeRound = false;
    public float effectLength = 0;

    protected float effectTimer = 0;
}
