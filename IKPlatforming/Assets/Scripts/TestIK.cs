using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class TestIK : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandObj = null;
    public Transform lookObj = null;

    [Range(0, 1f)]
    public float ikWeight;
    [Range(0, 1)]
    public int ikCalc;

    private Vector3 objectPosition;
    private Quaternion objectRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
        objectPosition = rightHandObj.position;
        objectRotation = rightHandObj.rotation;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            // Set the look target position, if one has been assigned
            if (lookObj != null)
            {
                animator.SetLookAtWeight(ikWeight);
                animator.SetLookAtPosition(lookObj.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
                if ( ikCalc == 1 )
                {
                    objectPosition = rightHandObj.position;
                    objectRotation = rightHandObj.rotation;
                }

                animator.SetIKPosition(AvatarIKGoal.LeftHand, objectPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, objectRotation);
            }
        }
    }
}
