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

    private Vector3 leftFootPosition;
    private Vector3 rightFootPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        enviroLayer = LayerMask.GetMask("Environment");
        player = LayerMask.GetMask("Player");

        UpdateIKFootPosition(AvatarIKGoal.LeftFoot);
        UpdateIKFootPosition(AvatarIKGoal.RightFoot);
    }


    private void OnAnimatorIK(int layerIndex)
    {
        float leftFootWeight = animator.GetFloat("IKLeftFootWeight");
        float rightFootWeight = animator.GetFloat("IKRightFootWeight");

        float leftFootCalc = animator.GetFloat("IKLeftFootCalc");
        float rightFootCalc = animator.GetFloat("IKRightFootCalc");

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);

        if ( leftFootCalc == 1.0f )
        {
            Debug.Log("Updating");
            UpdateIKFootPosition(AvatarIKGoal.LeftFoot);
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

        if ( rightFootCalc == 1.0f )
        {
            UpdateIKFootPosition(AvatarIKGoal.RightFoot);
        }
    }
    

    private void OnDrawGizmos()
    {
        //Ray gizmoRay = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(gizmoRay);
    }

    private void UpdateIKFootPosition(AvatarIKGoal foot)
    {
        RaycastHit hit;
        Ray toGround = new Ray(animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(foot, footPosition);
            animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }

    /*private void updateLeftIKFootPosition()
    {
        RaycastHit hit;
        Ray toGround = new Ray(animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(foot, footPosition);
            animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
    }*/
}
