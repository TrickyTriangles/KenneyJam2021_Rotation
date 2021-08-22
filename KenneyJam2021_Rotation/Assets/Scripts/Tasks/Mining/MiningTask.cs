using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using StateMachine._MiningTask;

public class MiningTask : Task, ICompleteable
{
    [SerializeField] private TaskProfile profile;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    private StateMachine<MiningTaskBaseState> state_machine;

    public Animator anim
    {
        get { return animator; }
    }

    private void Start()
    {
        state_machine = new StateMachine<MiningTaskBaseState>(this, new MiningTaskBaseState());

        if (profile != null)
        {
            vitality = profile.base_vitality;
        }
    }

    void ICompleteable.DoTask()
    {
        vitality--;

        if (vitality <= 0)
        {

        }
    }
}
