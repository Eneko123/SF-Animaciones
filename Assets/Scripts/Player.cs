using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum States {Idel, Walking, Runing}

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.2f;

    private bool canPlayerMove = true;
    private bool canCameraMove = true;
    private bool runing = false;

    private CharacterController controller;
    private Animator animator;
    private Vector2 moveInput;
    private float verticalVelocity;
    private float actualSpeed;
    private const float Gravity = -9.8f;
    private const float GroundedStick = -2f;

    private States state;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        state = States.Idel;
    }

    private void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = GroundedStick;
        else
            verticalVelocity += Gravity * Time.deltaTime;

        float speed = runing ? runSpeed : moveSpeed;

        Vector3 move =
            (transform.right * moveInput.x +
             transform.forward * moveInput.y) * speed;

        move.y = verticalVelocity;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        actualSpeed = horizontalVelocity.magnitude;

        animator.SetFloat("Vel", actualSpeed);
        animator.SetFloat("VerticalVel", verticalVelocity);

        controller.Move(move * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        moveInput = canPlayerMove ? value.Get<Vector2>() : Vector2.zero;
    }

    private void OnRun(InputValue value)
    {

        runing = value.isPressed;
    }

    private void OnJump(InputValue value)
    {
        if (canPlayerMove && value.isPressed && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Gravity);
    }

    public bool GetCanPlayerMove() => canPlayerMove;
    public bool GetCanCameraMove() => canCameraMove;
    public void SetCanPlayerMove(bool newCanPlayerMove) => canPlayerMove = newCanPlayerMove;
    public void SetCanCameraMove(bool newCanCameraMove) => canCameraMove = newCanCameraMove;
}