using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlimeController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;
    Vector2 direction;

    public float speed = .5f;
    float maxTimerLength = 3.0f;
    float timer;
    bool moving = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        // timer = maxTimerLength * Random.value;
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

        if (moving)
        {
            animator.SetFloat("movementInput.x", direction.x);
            animator.SetFloat("movementInput.y", direction.y);
            animator.SetFloat("speed", direction.magnitude);
        }

        if (!moving)
        {
            animator.SetFloat("speed", 0);
        }
    }

    void FixedUpdate()
    {
        if (moving)
        {
            Vector2 position = rigidbody2d.position;

            position.x = position.x + speed * direction.x * Time.deltaTime;
            position.y = position.y + speed * direction.y * Time.deltaTime;

            rigidbody2d.MovePosition(position);
        }

    }
}
