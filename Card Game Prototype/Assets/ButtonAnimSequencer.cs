using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimSequencer : MonoBehaviour
{
    List<Animator> _animators;
    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(DoAnim());
    }

    IEnumerator DoAnim()
    {
        while (true)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger("Bulge");
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}