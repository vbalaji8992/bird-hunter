using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool islodged;

    private GameObject collisionObject;

    private Quaternion rotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        islodged = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!islodged)
        {
            UpdateRotation();
        }

        if (collisionObject != null)
        {
            transform.position = collisionObject.transform.position;
            transform.rotation = collisionObject.transform.rotation * rotationOffset;
        }
    }

    public void Fire(Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), InputControl.Instance.GetComponent<Collider2D>());
    }

    private void UpdateRotation()
    {
        Vector2 v = rb2d.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameControl.Instance.arrowsInAir -= 1;

        DestroyComponents();

        islodged = true;

        if (collision.gameObject.name.Contains("BirdEnemy"))
        {
            rotationOffset = transform.rotation;
            collisionObject = collision.gameObject;
        }

        if (collision.gameObject.name.Contains("Plank"))
        {
            
        }
    }

    private void DestroyComponents()
    {
        Destroy(GetComponent<TrailRenderer>());
        Destroy(GetComponent<Collider2D>());
        Destroy(rb2d);
    }
}
