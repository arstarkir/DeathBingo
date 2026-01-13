using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float speed = 5;
    public float sprintSpeed = 7.5f;

    Vector2 inputVec;
    bool isSprinting = false;
    Rigidbody rb;

    public bool isInteractable = true;

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

    void PlayerMove() // This is a very temp thing (feel free to change it)
    {
        rb.linearVelocity = (new Vector3(inputVec.x, 0, inputVec.y) * (isSprinting ? sprintSpeed : speed));
    }

    #region Input System Callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
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
