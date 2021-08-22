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
    [SerializeField] private Collider2D hitbox;
    private StateMachine<MiningTaskBaseState> state_machine;

    #region Properties

    public Animator anim
    {
        get { return animator; }
    }

    public SpriteRenderer Sprite
    {
        get { return sprite; }
    }

    public Collider2D Hitbox
    {
        get { return hitbox; }
    }

    #endregion

    private void Start()
    {
        state_machine = new StateMachine<MiningTaskBaseState>(this, new MiningTaskBaseState());
        state_machine.SetNextState(new Appear());

        if (profile != null)
        {
            vitality = profile.base_vitality;
        }
    }

    void ICompleteable.DoTask(UnitProfile unit)
    {
        vitality--;
        SoundManagerScript.PlaySound("Mining");

        GameManager.Instance.Money += profile.diamond_reward_per_tick + unit.MiningStrength;

        if (vitality <= 0)
        {
            TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(profile.type));
            hitbox.enabled = false;
            state_machine.SetNextState(new StateMachine._MiningTask.Defeat());
        }
    }

    public override TaskProfile GetTaskProfile()
    {
        return profile;
    }
}
