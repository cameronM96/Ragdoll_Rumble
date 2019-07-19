using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimSequencer : MonoBehaviour
{
    List<Animator> _animators;
    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(DoAnim());
    }

    IEnumerator DoAnim() {
        while (true)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger("Animate");
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
