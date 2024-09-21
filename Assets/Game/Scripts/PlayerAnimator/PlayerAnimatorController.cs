using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (characterMovement)
        {
            animator.SetBool(PlayerAnimatorConstants.IsRunning, characterMovement.IsRunning);
            animator.SetBool(PlayerAnimatorConstants.IsJumping, characterMovement.IsJumping);
            animator.SetBool(PlayerAnimatorConstants.IsFalling, characterMovement.IsFalling);
        }        
    }
}
