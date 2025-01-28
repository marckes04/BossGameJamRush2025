using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboState
{
    NONE,
    ATTACK_1,
    ATTACK_2,
    ATTACK_3,
}

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerAsset;

    private InputAction attack;
    private InputAction special;

    private characterAnimations playerAnimations;

    private bool activateTimerToReset;
    private float defaultComboTimer = 0.4f;
    private float currentComboTimer;

    private ComboState currentComboState;

    private void Awake()
    {
        playerAsset = new PlayerController();
        playerAnimations = GetComponentInChildren<characterAnimations>();
    }

    private void OnEnable()
    {
        // Assign input actions
        attack = playerAsset.PlayerControls.Attack;
        special = playerAsset.PlayerControls.Special;

        playerAsset.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        playerAsset.PlayerControls.Disable();
    }

    private void Start()
    {
        currentComboTimer = defaultComboTimer;
        currentComboState = ComboState.NONE;
    }

    private void Update()
    {
        HandleComboAttacks();
        ResetComboState();
        HandleSpecialAttack();
    }

    private void HandleComboAttacks()
    {
        if (attack.WasPressedThisFrame())
        {
            // Cycle through combo states
            currentComboState++;
            activateTimerToReset = true;
            currentComboTimer = defaultComboTimer;

            // Trigger animations based on the current combo state
            if (currentComboState == ComboState.ATTACK_1)
            {
                playerAnimations?.Attack1();
            }
            else if (currentComboState == ComboState.ATTACK_2)
            {
                playerAnimations?.Attack2();
                currentComboState = ComboState.NONE; // Reset combo after final attack
            }
          
        }
    }

    private void ResetComboState()
    {
        if (activateTimerToReset)
        {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0f)
            {
                currentComboState = ComboState.NONE;
                activateTimerToReset = false;
                currentComboTimer = defaultComboTimer;
            }
        }
    }

    private void HandleSpecialAttack()
    {
        if (special.WasPressedThisFrame())
        {
            playerAnimations?.Attack3();
        }
    }
}
