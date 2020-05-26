﻿using System.Collections;
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
        } else if (controller.isGrounded == false) {
           moveDirection.y += (-gravity * Time.deltaTime);
        }

        //Move character
        controller.Move(moveDirection * Time.deltaTime);

        //Animations
        //
        if ( Input.GetAxis("Vertical") > 0 && controller.isGrounded == true)
        {
            //animator.SetFloat("VerticalAxis", Input.GetAxis("Vertical"));
            //animator.SetFloat("HorizontalAxis", Input.GetAxis("Horizontal"));
            animator.SetInteger("WalkDirection",1);
        }
        else if ( Input.GetAxis("Vertical") < 0 && controller.isGrounded == true)
        {
            // Placeholder for walking backwards
            animator.SetInteger("WalkDirection", -1);
        }
        else if(Input.GetAxis("Vertical") == 0)
        {
            //animator.SetBool("Walk", false);
            animator.SetInteger("WalkDirection", 0);
        }

    }
}
