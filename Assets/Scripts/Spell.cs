using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private Rigidbody2D rigi;
    private Transform target;

    [SerializeField] private float speed = 1f;

 
    void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        //디버깅용
        target = GameObject.Find("target").transform;
    }

    private void FixedUpdate()
    {
        Vector2 direction = target.position - transform.position;
        rigi.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
