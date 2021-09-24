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
    [SerializeField] private Collider2D hitbox;
    private Rigidbody2D rb;

    [SerializeField] private Task active_task;
    private Direction last_direction;
    private bool alive = true;
    private int vitality;

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

    public bool Alive
    {
        get { return alive; }
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

        vitality = profile.base_vitality;
    }

    private void GameManager_GameEnd()
    {
        if (alive)
        {
            state_machine.SetNextState(new Idle());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive)
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (alive)
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
        if (alive)
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
    }

    public void LoseVitality(int number)
    {
        if (alive)
        {
            vitality -= number;

            if (vitality <= 0)
            {
                alive = false;
                hitbox.enabled = false;
                state_machine.SetNextState(new StateMachine.Player_Unit.Defeat());
            }
        }
    }

    public void HandleClick()
    {
        if (alive)
        {
            UnitClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangeState(PlayerUnitBaseState state)
    {
        if (alive)
        {
            state_machine.SetNextState(state);
        }
    }
}