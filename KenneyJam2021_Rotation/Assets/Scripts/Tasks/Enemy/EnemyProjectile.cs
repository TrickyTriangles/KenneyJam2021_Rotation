using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float velocity;
    private Rigidbody2D rb;
    private Vector3 current_angle;

    private void Start()
    {
        current_angle = transform.rotation.eulerAngles;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        current_angle = Vector3.MoveTowards(current_angle, Vector3.zero, velocity * Time.deltaTime);
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
