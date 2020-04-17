using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpSpeed = 8.0f;
    public float rotationSpeed = 240.0f;
    public float gravity = 20.0f;
    public Animator playerAnimator;
    public List<Attack> attackList;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 rotation;
    private CharacterController charController;
    private PlayerAttackSystem attackSystem;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerAnimator = gameObject.GetComponent<Animator>();
        attackSystem = GetComponent<PlayerAttackSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        // Get Input for axis
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate the forward vector
        Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

        if (move.magnitude > 1f) move.Normalize();

        // Calculate the rotation for the player
        move = transform.InverseTransformDirection(move);

        // Get Euler angles
        float turnAmount = Mathf.Atan2(move.x, move.z);

        transform.Rotate(0, turnAmount * rotationSpeed * Time.deltaTime, 0);

        if (charController.isGrounded)
        {
            moveDirection = transform.forward * move.magnitude;
            moveDirection *= speed;

            playerAnimator.SetFloat("VerticalSpeed", moveDirection.z);
            playerAnimator.SetFloat("HorizontalSpeed", moveDirection.x);

            if (Input.GetButton("Jump"))
            {
                playerAnimator.SetTrigger("Jump");
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        charController.Move(moveDirection * Time.deltaTime);

    }

}
