using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using StateMachine.Enemy;

public class EnemyTask : Task, ICompleteable
{
    private StateMachine<EnemyBaseState> state_machine;
    [SerializeField] private TaskProfile profile;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    public Animator anim
    {
        get { return animator; }
    }

    private void Start()
    {
        state_machine = new StateMachine<EnemyBaseState>(this, new EnemyBaseState());
    }

    void ICompleteable.DoTask()
    {

    }
}
