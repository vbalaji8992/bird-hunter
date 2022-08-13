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
    protected float currentAngle;
    protected Vector2 initialPosition;

    public float movementSpeed;    
    public float radiusX;
    public float radiusY;    

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
        isDead = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePosition();
    }

    protected void UpdatePosition()
    {
        if (!isDead)
        {
            CalculateCurrentAngle();

            Move();

            //Vector2 offset = new Vector2(Mathf.Cos(currentAngle) * radiusX, Mathf.Sin(movementDirection * currentAngle) * radiusY);
            //transform.position = initialPosition + offset;


        }
    }

    protected void CalculateCurrentAngle()
    {
        currentAngle += movementSpeed * Time.deltaTime;

        if (currentAngle >= twoPiRadians)
        {
            currentAngle -= twoPiRadians;
        }
    }

    protected virtual void Move()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {       

        if (collision.gameObject.name.Contains("Arrow"))
        {
            GameControl.Instance.currentEnemies -= 1;
            isDead = true;
            anim.SetTrigger("BirdDead");
            rb2d.gravityScale = 1.0f;
        }       
    }
}
