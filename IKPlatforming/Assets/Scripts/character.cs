using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour{

    public float speed = 6.0f;
    public float rotateSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 30.0f;

    public Animator animator;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private int falling = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

      print(controller.isGrounded);

      //Executes when character is on the ground
      if(controller.isGrounded){
        //Forward-Backwards movment using F/B Arrow Keys
        falling = 0;
        moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        //Rotation using R/L Arrow Keys
        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

     }

        //If jump is pressed, move upwards; elif falling, apply gravity.
        if (Input.GetButton("Jump")){
           moveDirection.y = jumpSpeed;
           //Disables walking animation while jumping up
           animator.SetBool("Walk", false);
           falling = 1;
        } else //if (controller.isGrounded == false)
        {
           moveDirection.y += (-gravity * Time.deltaTime);
        }

        print(moveDirection.y);

        //Move character
        controller.Move(moveDirection * Time.deltaTime);

        //Animations
        //
        if ( Input.GetAxis("Vertical") > 0 && moveDirection.y < 0.0f && falling != 1)
        {
            //print("case1");
            animator.SetBool("Walk",true);
        }
        else if ( Input.GetAxis("Vertical") < 0 && moveDirection.y < 0.0f && falling != 1)
        {
            //print("case2");
            // Placeholder for walking backwards
        }
         else //else if(Input.GetAxis("Vertical") <= 0)
        {
            //print("case3");
            animator.SetBool("Walk", false);
        }

    }
}
