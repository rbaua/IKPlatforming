using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointIK : MonoBehaviour
{
    protected Animator animator;

    [Range(0, 5)]
    public float armLength;

    LayerMask enviroLayer;
    LayerMask player;

    public bool ikActive = false;
    public Transform mesh;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enviroLayer = LayerMask.GetMask("Environment");
        player = LayerMask.GetMask("Player");
    }

    private void Update()
    {
      
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        Vector2 mousePosition = Input.mousePosition;
        Debug.Log("mouse: " + mousePosition);
        Quaternion pointRotation = transform.parent.rotation;
        Vector3 pointPosition = animator.GetIKPosition(AvatarIKGoal.RightHand) + (mesh.forward * armLength) + (new Vector3(mousePosition.x * Mathf.Sin(transform.parent.rotation.y), mousePosition.y - Screen.height/2, mousePosition.x * Mathf.Cos(transform.parent.rotation.y)) * (1f/Screen.width));
        Debug.Log("pointing at: " + pointPosition);

        
        //footPosition.y += distanceToGround;
        animator.SetIKPosition(AvatarIKGoal.RightHand, pointPosition);
        animator.SetIKRotation(AvatarIKGoal.RightHand, pointRotation);
    }
}
