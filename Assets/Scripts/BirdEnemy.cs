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
    protected bool isOnGround;
    protected Vector2 initialPosition;
    protected Vector2 previousPosition;

    public float movementSpeed;

    private List<Vector3> positions;
    private int currentPosIndex;

    // Start is called before the first frame update
    void Start()
    {
        InitializeObject();
        InitializePositions();
    }

    protected virtual void InitializePositions()
    {
        positions = new List<Vector3>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "BirdPosition")
                positions.Add(child.gameObject.transform.position);
        }
        currentPosIndex = 0;
    }

    protected void InitializeObject()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        initialPosition = transform.position;
        previousPosition = initialPosition;
        isDead = false;
        isOnGround = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            CheckDirectionAndFlipSprite();
        }               
    }

    protected virtual void Move()
    {
        if (positions.Count == 0)
            return;

        var target = positions[currentPosIndex];
        var diff = (Vector2)(target - transform.position);
        if (diff.magnitude > 1.05)
        {
            rb2d.MovePosition((Vector2)transform.position + diff.normalized * Time.deltaTime * movementSpeed);
        }
        else 
        {
            currentPosIndex += 1;
            if (currentPosIndex == positions.Count)
                currentPosIndex = 0;
        }
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
        if (collision.gameObject.tag == "Arrow" && !isDead)
        {
            GameControl.Instance.currentEnemies -= 1;
            isDead = true;
            anim.SetTrigger("BirdDead");
            rb2d.gravityScale = 1.0f;
        }

        if (collision.gameObject.tag != "Arrow" && isDead && !isOnGround)
        {
            isOnGround = true;
            GameControl.Instance.enemiesOnGround += 1;            
        }
    }
}
