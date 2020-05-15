using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour{

   public float speed = 6.0f;
   public float rotateSpeed = 6.0f;

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
        moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        //Rotation using R/L Arrow Keys
        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        //Gravity Not Implemented
        //moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
