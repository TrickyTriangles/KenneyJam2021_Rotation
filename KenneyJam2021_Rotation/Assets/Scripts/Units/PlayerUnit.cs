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
    [SerializeField] private GameObject projectile;
    private Rigidbody2D rb;

    [SerializeField] private Task active_task;
    private Direction last_direction;

    #region Properties

    public UnitProfile Profile
    {
        get { return profile; }
    }

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

    public GameObject Projectile
    {
        get { return projectile; }
    }

    public Direction LastDirection
    {
        get { return last_direction; }
    }

    #endregion

    #region Delegates and Subscriber Methods

        private EventHandler UnitClicked;
        public void Subscribe_UnitClicked(EventHandler sub) { UnitClicked += sub; }
        public void Unsubscribe_UnitClicked(EventHandler sub) { UnitClicked -= sub; }

    #endregion


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
            if (active_task == null)
            {
                Task task = collision.gameObject.GetComponentInParent<Task>();
                active_task = task;
                state_machine.SetNextState(new DoTask(active_task));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (active_task != null)
        {
            if (collision.gameObject == active_task.gameObject)
            {
                active_task = null;
                state_machine.SetNextState(new Move(last_direction));
            }
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

        last_direction = new_direction;
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