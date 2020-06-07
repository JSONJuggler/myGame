using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlimeController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;
    Vector2 direction;
    Vector2 boxSize = new Vector2(5, 5);
    Vector2 playerDirection;

    public float speed = 10f;
    public float seekSpeed = 15f;
    float maxTimerLength = 3.0f;
    float timer;
    bool moving = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = maxTimerLength * Random.value;
        animator = GetComponent<Animator>();
        direction = Random.insideUnitCircle;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            if (moving)
            {
                direction = Random.insideUnitCircle;
                timer = 1.3f;
                moving = false;
                return;
            }
            if (!moving)
            {
                direction = Random.insideUnitCircle;
                timer = maxTimerLength * Random.value;
                moving = true;
                return;
            }
        }

        timer = timer - Time.deltaTime;
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast(rigidbody2d.position, boxSize, 0.0f, new Vector2(0, 0), 0f, LayerMask.GetMask("Player"));

        if (hit.collider != null)
        {
            Rigidbody2D player = hit.collider.GetComponent<Rigidbody2D>();
            playerDirection = player.position - rigidbody2d.position;

            animator.SetBool("seeking", true);
            animator.SetFloat("movementInput.x", playerDirection.x);
            animator.SetFloat("movementInput.y", playerDirection.y);
            rigidbody2d.AddForce(playerDirection * speed * Time.deltaTime, ForceMode2D.Impulse);

            return;
        }

        if (moving)
        {
            animator.SetBool("seeking", false);
            animator.SetFloat("movementInput.x", direction.x);
            animator.SetFloat("movementInput.y", direction.y);
            animator.SetFloat("speed", direction.magnitude);

            rigidbody2d.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Impulse);
        }

        if (!moving)
        {
            animator.SetFloat("speed", 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.ChangeHealth(-1);
            }

            Rigidbody2D player = other.gameObject.GetComponent<Rigidbody2D>();
            player.AddForce(playerDirection * 20, ForceMode2D.Impulse);
        }
    }
}
