using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Rigidbody2D rb;
    private UnitProfile unit;
    private Task target;
    private Vector3 destination;

    public void Initialize(Task _target, UnitProfile _unit)
    {
        unit = _unit;
        target = _target;
        target.Subscribe_TaskCompleted(Task_TaskComplete);
        destination = target.transform.rotation.eulerAngles;
        SoundManagerScript.PlaySound("BulletShot");
    }

    private void Task_TaskComplete(object sender, System.EventArgs args)
    {
        target = null;
    }

    private void FixedUpdate()
    {
        if (target != null) { destination = target.transform.rotation.eulerAngles; }

        Vector3 new_rot = new Vector3(0f, 0f, Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, destination.z, velocity * Time.fixedDeltaTime));
        rb.SetRotation(Quaternion.Euler(new_rot));

        if (Vector3.Distance(new_rot, destination) < 0.1f)
        {
            if (target != null)
            {
                ICompleteable comp = target;
                comp.DoTask(unit);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
