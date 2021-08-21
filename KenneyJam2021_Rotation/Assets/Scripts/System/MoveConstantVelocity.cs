using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveConstantVelocity : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Vector3 direction;

    private void Start()
    {
        direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += direction * velocity * Time.deltaTime;
    }
}
