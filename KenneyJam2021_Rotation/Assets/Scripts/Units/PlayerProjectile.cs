using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Rigidbody2D rb;
    private Task target;
    private Vector3 destination;

    public void Initialize(Task _target)
    {
        target = _target;
        target.Subscribe_TaskCompleted(Task_TaskComplete);
        destination = target.transform.rotation.eulerAngles;
    }

    private void Task_TaskComplete(object sender, System.EventArgs args)
    {
        target = null;
    }

    private void FixedUpdate()
    {
        if (target != null) { destination = target.transform.rotation.eulerAngles; }

        Vector3 new_rot = Vector3.MoveTowards(transform.rotation.eulerAngles, destination, velocity * Time.fixedDeltaTime);
        rb.SetRotation(Quaternion.Euler(new_rot));

        if (Vector3.Distance(new_rot, destination) < 0.1f)
        {
            if (target != null)
            {
                ICompleteable comp = target;
                comp.DoTask();
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
