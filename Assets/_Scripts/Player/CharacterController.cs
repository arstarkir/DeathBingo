using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : Singleton<CharacterController>
{
    public float speed = 5;
    public float sprintSpeed = 7.5f;

    [HideInInspector] public Vector2 inputVec;
    bool isSprinting = false;
    Rigidbody rb;

    public bool isInteractable = true;
    [HideInInspector] public Vector3 pushVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isInteractable)
            return;

        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 moveVel = new Vector3(inputVec.x, 0, inputVec.y) * (isSprinting ? sprintSpeed : speed);
        rb.linearVelocity = moveVel + pushVelocity;
    }

    #region Input System Callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!isInteractable)
            return;
        inputVec = ctx.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        isSprinting = !isSprinting;
    }

    public void OnJump()
    {

    }
    #endregion
}
