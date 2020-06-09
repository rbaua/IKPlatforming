using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKControl : MonoBehaviour
{
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

    private Vector3 leftFootPosition;
    private Quaternion leftFootRotation;
    private float leftLegLength;

    private Vector3 rightFootPosition;
    private Quaternion rightFootRotation;
    private float rightLegLength;

    public Material playerMaterial;

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

        if (leftFootWeight > .9)
        {
            playerMaterial.SetInt("Boolean_8A146678", 1);
            playerMaterial.SetVector("Vector3_17EC27E9", animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        }
        else
        {
            playerMaterial.SetInt("Boolean_8A146678", 0);
        }

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);


        if ( rightFootCalc == 1.0f )
        {
            UpdateRightFootPosition();
        }

        if (rightFootWeight > .9)
        {
            playerMaterial.SetInt("Boolean_6D8D6C42", 1);
            playerMaterial.SetVector("Vector3_821C0DB5", animator.GetIKPosition(AvatarIKGoal.RightFoot));
        }
        else
        {
            playerMaterial.SetInt("Boolean_6D8D6C42", 0);
        }

        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);

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
        Ray toGround = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + legLength * Vector3.up + new Vector3(0,1f,0), Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            leftLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            leftFootPosition = footPosition;
            leftFootRotation = Quaternion.LookRotation(transform.forward, hit.normal);
        }
    }

    private void UpdateRightFootPosition()
    {
        RaycastHit hit;
        Ray toGround = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + legLength * Vector3.up + new Vector3(0, 1f, 0), Vector3.down);
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength + 1f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            rightLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            rightFootPosition = footPosition;
            rightFootRotation = Quaternion.LookRotation(transform.forward, hit.normal);
        }
    }

    private void ResetFigureHeight()
    {
        transform.position = transform.parent.transform.position;
    }

    private void UpdateFigureHeight()
    {
        float unbentLegLength = Mathf.Max(leftLegLength, rightLegLength);
        Debug.Log(unbentLegLength);
        float torsoDisplacement = legLength - unbentLegLength - .1f;

        transform.position = transform.parent.transform.position + new Vector3(0, torsoDisplacement, 0);
    }
}
