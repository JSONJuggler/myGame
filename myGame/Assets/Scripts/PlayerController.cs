using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public int atk;
    public int def;
    public int mag;
    public float speed;

    Rigidbody2D rigidbody2d;
    PlayerInputActions inputAction;
    Vector2 move = new Vector2(0, 0);
    Vector2 lookDirection = new Vector2(1, 0);
    Vector2 movementInput;
    Animator animator;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position = position + movementInput * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    void Update()
    {
        movementInput = inputAction.Player.Move.ReadValue<Vector2>();

        move.Set(movementInput.x, movementInput.y);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("movementInput.x", lookDirection.x);
        animator.SetFloat("movementInput.y", lookDirection.y);
        animator.SetFloat("speed", movementInput.magnitude);
    }
}