using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEnemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;

    public bool IsDead { get; protected set; }

    protected bool isOnGround;
    protected Vector2 initialPosition;
    protected Vector2 previousPosition;

    public float movementSpeed;

    private List<Vector3> positions;
    private int currentPosIndex;

    protected GameObject deathArrow;
    protected GameControl gameControl;

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
        IsDead = false;
        isOnGround = false;

        gameControl = GameControl.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsDead)
        {
            Move();
            CheckDirectionAndFlipSprite();
        }

        if (isOnGround)
            HandleDeadBird();
    }

    protected virtual void HandleDeadBird()
    {

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
        if (collision.gameObject.tag == "Arrow" && !IsDead)
        {
            UpdateKillCount();
            IsDead = true;
            anim.SetTrigger("BirdDead");
            rb2d.gravityScale = 1.0f;
            deathArrow = collision.gameObject;
        }

        if (collision.gameObject.tag != "Arrow" && IsDead && !isOnGround)
        {
            isOnGround = true;
            gameControl.EnemiesOnGround += 1;
        }
    }

    protected virtual void UpdateKillCount()
    {
        gameControl.CurrentEnemies -= 1;
    }
}
