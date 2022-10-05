using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
    protected const float twoPiRadians = 6.28319f;

    protected Animator anim;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;

    protected bool isDead;
    protected Vector2 initialPosition;
    protected Vector2 previousPosition;

    public float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        InitializeObject();
    }

    protected void InitializeObject()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
        previousPosition = initialPosition;
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();

        CheckDirectionAndFlipSprite();
    }

    protected void UpdatePosition()
    {
        if (!isDead)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
       
    }

    private void CheckDirectionAndFlipSprite()
    {
        if (transform.position.x > previousPosition.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        previousPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Arrow") && !isDead)
        {
            GameControl.Instance.currentEnemies -= 1;
            isDead = true;
            anim.SetTrigger("BirdDead");
            rb2d.gravityScale = 1.0f;
        }       
    }
}
