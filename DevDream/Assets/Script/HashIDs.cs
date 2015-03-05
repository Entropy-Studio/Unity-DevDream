using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
    public int groundBool;
    public int speedFloat;
    public int crouchBool;
    public int vSpeedFloat;
    public int wallSlideBool;
    public int deadBool;
    public int jumpRecoveryBool;

    void Awake()
    {
        groundBool = Animator.StringToHash("Ground");
        speedFloat = Animator.StringToHash("Speed");
        crouchBool = Animator.StringToHash("Crouch");
        vSpeedFloat = Animator.StringToHash("vSpeed");
        wallSlideBool = Animator.StringToHash("WallSlide");
        deadBool = Animator.StringToHash("Dead");
        jumpRecoveryBool = Animator.StringToHash("Jump Recovery");
    }
}
