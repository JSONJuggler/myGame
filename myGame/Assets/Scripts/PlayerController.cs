using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject playerInfo;
    bool playerInfoOpen = false;
    int currentHealth;
    int currentAtk;
    int currentDef;
    int currentMag;
    int currentDex;
    public int maxHealth;
    public int atk;
    public int def;
    public int mag;
    public int dex;
    public float speed;

    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI hpBar;
    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI defStat;
    public TextMeshProUGUI magStat;
    public TextMeshProUGUI dexStat;

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
        currentAtk = atk;
        currentDef = def;
        currentMag = mag;
        currentDex = dex;
        hpStat.text = "Health: " + currentHealth + "/" + maxHealth;
        hpBar.text = hpStat.text;
        atkStat.text = "Attack: " + currentAtk;
        defStat.text = "Defense: " + currentDef;
        magStat.text = "Magic: " + currentMag;
        dexStat.text = "Dexterity: " + currentDex;

        animator = GetComponent<Animator>();
        playerInfo.SetActive(false);
    }

    void Awake()
    {
        inputAction = new PlayerInputActions();
        inputAction.UI.OpenPlayerInfo.performed += ctx => TogglePlayerInfo(ctx.ReadValue<float>());

    }

    void FixedUpdate()
    {
        rigidbody2d.AddForce(movementInput * speed * Time.deltaTime, ForceMode2D.Impulse);
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
        if (Time.timeScale > 0)
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

    void TogglePlayerInfo(float pressed)
    {
        if (pressed == 1)
        {
            if (playerInfoOpen)
            {
                playerInfoOpen = false;
                playerInfo.SetActive(false);
                Time.timeScale = 1;
                return;
            }

            if (!playerInfoOpen)
            {
                playerInfoOpen = true;
                playerInfo.SetActive(true);
                Time.timeScale = 0;
                return;
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        hpStat.text = "Health: " + currentHealth + "/" + maxHealth;
        hpBar.text = hpStat.text;
    }
}