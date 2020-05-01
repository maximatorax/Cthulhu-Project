using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintMultiplier = 2.0f;
    public bool isSprinting;
    public float jumpSpeed = 8.0f;
    public float rotationSpeed = 240.0f;
    public float gravity = 20.0f;
    public Animator playerAnimator;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController charController;
    private PlayerAttackSystem playerAttackSystem;
    private bool canSprint;
    private bool activeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        charController = gameObject.GetComponent<CharacterController>();
        playerAttackSystem = gameObject.GetComponent<PlayerAttackSystem>();
        playerAnimator = gameObject.GetComponent<Animator>();
        isSprinting = false;
        canSprint = true;
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

        if (playerAttackSystem.stamina >= 10)
        {
            canSprint = true;
        }

        if (charController.isGrounded)
        {
            moveDirection = transform.forward * move.magnitude;

            if (Input.GetButtonUp("Sprint"))
            {
                StopCoroutine(Sprint());
                isSprinting = false;
            }

            if (Input.GetButton("Sprint") && !isSprinting && playerAttackSystem.stamina >= 10 && moveDirection != Vector3.zero)
            {
                StartCoroutine(Sprint());
            }
            else if(Input.GetButton("Sprint") && moveDirection == Vector3.zero)
            {
                isSprinting = false;
                StopCoroutine(Sprint());
            }

            if (isSprinting)
            {
                moveDirection *= speed * sprintMultiplier;
            }
            else
            {
                moveDirection *= speed;
            }

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

    IEnumerator Sprint()
    {
        if(activeCoroutine) yield break;

        isSprinting = true;
        activeCoroutine = true;

        if (playerAttackSystem.stamina < 10)
        {
            canSprint = false;
            isSprinting = false;
            activeCoroutine = false;
            yield break;
        }

        while (canSprint && isSprinting && playerAttackSystem.stamina >=10)
        {
            yield return new WaitForSeconds(0.5f);
            if (playerAttackSystem.stamina < 10)
            {
                canSprint = false;
                isSprinting = false;
                activeCoroutine = false;
                yield break;
            }
            if (playerAttackSystem.stamina < 0)
            {
                playerAttackSystem.stamina = 0;
            }
            playerAttackSystem.stamina -= 10;
            
            
            yield return new WaitForSeconds(0.5f);
        }
        activeCoroutine = false;
        isSprinting = false;
    }

}
