using System;
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
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private GameObject projectile;
    private Rigidbody2D rb;

    public Animator anim
    {
        get { return animator; }
    }

    public Collider2D Hitbox
    {
        get { return hitbox; }
    }

    public GameObject Projectile
    {
        get { return projectile; }
    }

    public Rigidbody2D RigidBody
    {
        get { return rb; }
    }

    private void Start()
    {
        state_machine = new StateMachine<EnemyBaseState>(this, new EnemyBaseState());
        state_machine.SetNextState(new Appear());
        Subscribe_TaskCompleted(Task_OnComplete);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Task_OnComplete(object sender, EventArgs args)
    {
        state_machine.SetNextState(new Defeat());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            state_machine.SetNextState(new Idle());
        }
    }

    void ICompleteable.DoTask()
    {
        vitality--;

        if (vitality <= 0)
        {
            TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(profile.type));
        }
    }

    public void SetDirection(Move.Direction new_direction)
    {
        if (sprite != null)
        {
            switch (new_direction)
            {
                case Move.Direction.RIGHT:
                    sprite.flipX = true;
                    break;
                case Move.Direction.LEFT:
                    sprite.flipX = false;
                    break;
                default:
                    break;
            }
        }
    }
}
