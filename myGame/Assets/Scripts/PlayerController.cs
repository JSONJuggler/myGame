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
    Vector2 movementInput;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
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

    }
}