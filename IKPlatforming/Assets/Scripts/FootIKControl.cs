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
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

        //left
        RaycastHit hit;
        Ray toGroundLeft = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if(Physics.Raycast(toGroundLeft, out hit, distanceToGround + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

        //left
        Ray toGroundRight = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(toGroundRight, out hit, distanceToGround + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
        }
    }
}
