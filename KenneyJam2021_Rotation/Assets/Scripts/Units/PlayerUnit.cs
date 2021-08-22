using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine.Player_Unit;
using System;

public class PlayerUnit : MonoBehaviour
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }

    private StateMachine.StateMachine<PlayerUnitBaseState> state_machine;
    [SerializeField] private UnitProfile profile;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    public Animator anim
    {
        get { return animator; }
    }

    public float velocity
    {
        get { return profile.velocity; }
    }

    public Rigidbody2D RigidBody
    {
        get { return rb; }
    }

    private EventHandler UnitClicked;
    public void Subscribe_UnitClicked(EventHandler sub) { UnitClicked += sub; }
    public void Unsubscribe_UnitClicked(EventHandler sub) { UnitClicked -= sub; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        state_machine = new StateMachine.StateMachine<PlayerUnitBaseState>(this, new PlayerUnitBaseState());
        BeginMovement();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Subscribe_GameEnd(GameManager_GameEnd);
        }
    }

    private void GameManager_GameEnd()
    {
        state_machine.SetNextState(new Idle());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Task"))
        {
            Task task = collision.gameObject.GetComponent<Task>();
        }
    }

    public void SetDirection(Direction new_direction)
    {
        if (sprite != null)
        {
            switch (new_direction)
            {
                case Direction.LEFT:
                    sprite.flipX = true;
                    break;
                case Direction.RIGHT:
                    sprite.flipX = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void BeginMovement()
    {
        float number = UnityEngine.Random.Range(-10f, 10f);

        if (number > 0)
        {
            state_machine.SetNextState(new Move(Direction.LEFT));
        }
        else
        {
            state_machine.SetNextState(new Move(Direction.RIGHT));
        }
    }

    public void HandleClick()
    {
        UnitClicked?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeState(PlayerUnitBaseState state)
    {
        state_machine.SetNextState(state);
    }
}