using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class Move : EnemyBaseState
    {
        public enum Direction
        {
            LEFT, RIGHT
        }

        private EnemyTask task;
        private Direction direction;
        private float velocity = 7f;
        private float shot_delay = 3f;
        private Vector3 current_angle;

        public Move(Direction _dir)
        {
            direction = _dir;
        }

        public override IEnumerator ProcessState(MonoBehaviour subject, Action<BaseState> next_state_callback)
        {
            task = subject as EnemyTask;

            task.SetDirection(direction);
            task.anim.Play("Move", 0);
            current_angle = task.transform.rotation.eulerAngles;

            float dir = direction == Direction.LEFT ? 1f : -1f;
            float timer = 0f;

            while (true)
            {
                current_angle = Vector3.MoveTowards(current_angle, Vector3.zero, velocity * Time.deltaTime);
                task.RigidBody.SetRotation(Quaternion.Euler(current_angle));
                timer += Time.deltaTime;

                if (timer > shot_delay)
                {
                    if (task.Projectile != null)
                    {
                        GameObject.Instantiate(task.Projectile, Vector3.zero, task.transform.rotation);
                    }

                    timer = 0f;
                }

                yield return null;
            }

        }
    }
}