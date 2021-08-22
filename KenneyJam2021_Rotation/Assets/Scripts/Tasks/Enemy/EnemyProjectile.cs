using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float velocity;
    private Rigidbody2D rb;
    private float start_angle;
    private Vector3 current_angle;

    private void Start()
    {
        current_angle = transform.rotation.eulerAngles;
        start_angle = transform.rotation.eulerAngles.z;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float dir = start_angle > 180f ? 1f : -1f;

        current_angle.z += velocity * dir * Time.deltaTime;
        rb.SetRotation(Quaternion.Euler(current_angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            Objective obj = collision.gameObject.GetComponent<Objective>();

            if (obj != null)
            {
                obj.HitObject();
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Unit"))
        {
            // Damage unit
            Destroy(gameObject);
        }
    }
}
