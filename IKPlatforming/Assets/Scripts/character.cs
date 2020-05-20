using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour{

    public float speed = 6.0f;
    public float rotateSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

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
        //Forward-Backwards movment using F/B Arrow Keys
        moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        //Rotation using R/L Arrow Keys
        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        if (Input.GetButton("Jump")){
           moveDirection.y = jumpSpeed;
        } else {
           moveDirection.y = -4.0f;
        }

        //Gravity
        //moveDirection.y -= (gravity * Time.deltaTime);
        //moveDirection.y = -4.0f;

        controller.Move(moveDirection * Time.deltaTime);


        if ( Input.GetAxis("Vertical") > 0 )
        {
            animator.SetBool("Walk",true);
        }
        else if ( Input.GetAxis("Vertical") > 0 )
        {
            // Placeholder for walking backwards
        }
        else
        {
            animator.SetBool("Walk", false);
        }

    }

    //code from AMartin223: https://answers.unity.com/questions/17566/how-can-i-make-my-player-a-charactercontroller-pus.html
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
            body.velocity += hit.controller.velocity;
    }
}
