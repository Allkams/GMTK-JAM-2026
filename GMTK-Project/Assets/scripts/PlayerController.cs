using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] CharacterController controller;
    [SerializeField] Slider staminaSlider;

    [Header("Player settings")]
    [SerializeField] float speed = 5.0f;
    [SerializeField] float runSpeed = 10.0f;
    [SerializeField] float runDuration = 60f;
    [SerializeField] float gravity =  -20.0f;
    [SerializeField] float sensitivity = 0.1f;

    private float pitch;

    private Vector2 moveInput;
    private Vector3 velocity;

    private float staminaValue = 100f;
    private float staminaStep;

    private bool isRunning;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        staminaStep = staminaValue / runDuration;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        UpdateStaminaSlider();

        SolveGravity();
    }

    private void SolveGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void MovePlayer()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        float moveSpeed = speed;

        if (isRunning && staminaValue > 0)
        {
            moveSpeed = runSpeed;
            staminaValue -= staminaStep * Time.deltaTime;
            if (staminaValue <= 0)
            {
                isRunning = false;
            }
        }

        if (staminaValue < 100 && !isRunning)
        {
            staminaValue += staminaStep*Time.deltaTime*1.5f;
        }

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

#region Handle Input
    public void Run(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            isRunning = true;
        }

        if(ctx.canceled)
        {
            isRunning = false;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        Vector2 delta = ctx.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * delta.x * sensitivity);

        pitch -= delta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

#endregion

    private void UpdateStaminaSlider()
    {
        staminaSlider.value = staminaValue;
    }
}
