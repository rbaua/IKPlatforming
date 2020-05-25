using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKControl : MonoBehaviour
{
    protected Animator animator;

    [Range(0,1f)]
    public float distanceToGround;

    LayerMask enviroLayer;
    LayerMask player;

    public bool ikActive = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enviroLayer = LayerMask.GetMask("Environment");
        player = LayerMask.GetMask("Player");
    }


    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));

        //left
        RaycastHit hitLeft;
        Ray toGroundLeft = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if(Physics.Raycast(toGroundLeft, out hitLeft, distanceToGround + 1f))
        {
            Vector3 footPosition = hitLeft.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hitLeft.normal));
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));

        //right
        RaycastHit hitRight;
        Ray toGroundRight = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(toGroundRight, out hitRight, distanceToGround + 1f, enviroLayer))
        {
            Vector3 footPosition = hitRight.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hitRight.normal));
        }
    }
    

    private void OnDrawGizmos()
    {
        //Ray gizmoRay = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(gizmoRay);
    }
}
