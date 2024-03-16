using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    public float sprintSpeed = 5;
    public float depleteSpeed = 2;

    public float jumpHeight = 3f;
    public float gravity = 9.81f;
    public float airControl = 1f;

    // amount of seconds a player can consecutively sprint
    public float sprintDuration = 5f;
    // the amount of seconds of sprint a player regains per second when not exhausted
    public float sprintRecoveryRate = 1f;
    // the amount of seconds of sprint a player regiains per second when exhausted
    public float depleteRecoveryRate = 1f;
    public Image sprintContent;

    CharacterController controller;

    Vector3 input, moveDirection;

    private float currentSpeed;

    private float sprintAmount;
    private bool exhausted;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
        sprintAmount = sprintDuration;
        exhausted = false;

        if (sprintContent == null)
        {
            sprintContent = GameObject.FindGameObjectWithTag("StaminaContent").GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !exhausted && sprintAmount >= 0f)
        {
            currentSpeed = sprintSpeed;
            sprintAmount -= Time.deltaTime;
            if (sprintAmount <= 0f)
            {
                sprintAmount = 0f;
                exhausted = true;
            }
        }
        else
        {
            if (exhausted)
            {
                currentSpeed = depleteSpeed;
                sprintAmount += Time.deltaTime * depleteRecoveryRate;
            }
            else
            {
                currentSpeed = moveSpeed;
                sprintAmount += Time.deltaTime * sprintRecoveryRate;
            }

            if (sprintAmount >= sprintDuration)
            {
                sprintAmount = sprintDuration;
                exhausted = false;
            }
        }

        sprintContent.fillAmount = sprintAmount / sprintDuration;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = transform.right * moveHorizontal + transform.forward * moveVertical;
        input = input.normalized * currentSpeed;

        if (controller.isGrounded) {
            moveDirection = input;
            if (Input.GetButton("Jump")) {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            } else {
                moveDirection.y = 0f;
            }
        } else {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
