using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D rigi;
    private Animator animator;
    public Transform MyTarget { get;private set; }

    [SerializeField] private float speed = 1f;
    private float damage;
    private bool alive;

 
    void Awake()
    {
        rigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position;
            rigi.velocity = direction.normalized * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HitBox") && collision.transform == MyTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            animator.SetTrigger("Impact");
            rigi.velocity = Vector2.zero;
            MyTarget = null;
        }
    }

    public void Initialize(Transform target, int damage)
    {
        this.MyTarget = target;
        this.damage = damage;
    }
}
