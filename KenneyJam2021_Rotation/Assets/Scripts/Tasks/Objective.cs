using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Objective : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int start_vitality;
    private int vitality;

    private Action ObjectiveDestroyed;
    public void Subscribe_ObjectiveDestroyed(Action sub) { ObjectiveDestroyed += sub; }
    public void Unsubscribe_ObjectiveDestroyed(Action sub) { ObjectiveDestroyed -= sub; }

    private void Start()
    {
        vitality = start_vitality;
    }

    public void HitObject()
    {
        vitality--;

        if (vitality <= 0)
        {
            animator.Play("Defeat", 0);
            ObjectiveDestroyed?.Invoke();
        }
        else
        {
            if (animator != null)
            {
                animator.Play("Hit", 0);
                animator.playbackTime = 0f;
            }
        }
    }
}
