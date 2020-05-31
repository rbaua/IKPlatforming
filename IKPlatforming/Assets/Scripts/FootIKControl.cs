using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKControl : MonoBehaviour
{
    // I need one fo the legs to equal legLength
    // Other leg needs to be in the range of 0 - legLength

    protected Animator animator;

    [Range(0,1f)]
    public float distanceToGround;
    [Range(0, 3f)]
    public float legLength;
    [Range(0, 5f)]
    public float capsuleBottomHeight;

    LayerMask enviroLayer;
    LayerMask player;

    public bool ikActive = false;
    // Start is called before the first frame update

    private Vector3 leftFootPosition;
    private Vector3 rightFootPosition;
    private float leftLegLength;
    private float rightLegLength;

    void Start()
    {
        animator = GetComponent<Animator>();
        enviroLayer = LayerMask.GetMask("Environment");
        player = LayerMask.GetMask("Player");

        //Physics.IgnoreCollision(this.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
    }


    private void OnAnimatorIK(int layerIndex)
    {
        float leftFootWeight = animator.GetFloat("IKLeftFootWeight");
        float rightFootWeight = animator.GetFloat("IKRightFootWeight");

        float leftFootCalc = animator.GetFloat("IKLeftFootCalc");
        float rightFootCalc = animator.GetFloat("IKRightFootCalc");

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);

        ResetFigureHeight();

        if ( leftFootCalc == 1.0f )
        {
            UpdateLeftFootPosition();
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

        if ( rightFootCalc == 1.0f )
        {
            UpdateRightFootPosition();
        }

        if(rightLegLength > 1f)
        {
            Debug.Log(rightLegLength);
        }

        UpdateFigureHeight();
    }
    

    private void OnDrawGizmos()
    {
        //Ray gizmoRay = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(gizmoRay);
    }

    private void UpdateLeftFootPosition()
    {
        RaycastHit hit;
        Ray toGround = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + legLength * Vector3.up, Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            leftLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
        else
        {
            leftLegLength = legLength;
        }
    }

    private void UpdateRightFootPosition()
    {
        RaycastHit hit;
        Ray toGround = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + legLength * Vector3.up, Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            rightLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
        }
        else
        {
            rightLegLength = legLength;
        }
    }

    private void ResetFigureHeight()
    {
        transform.position = transform.parent.transform.position;
    }

    private void UpdateFigureHeight()
    {
        float unbentLegLength = Mathf.Max(leftLegLength, rightLegLength);
        float torsoDisplacement = legLength - unbentLegLength - .1f;

        transform.position = transform.parent.transform.position + new Vector3(0, torsoDisplacement, 0);
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
