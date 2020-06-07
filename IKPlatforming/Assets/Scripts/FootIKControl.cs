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

    public Vector3 rightFootPosition;
    public Quaternion rightFootRotation;
    public float rightLegLength;

    private Ray rightFootRay;

    public GameObject leftFootBone;
    public GameObject rightFootBone;

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

        if(leftFootWeight > .5)
        {
            playerMaterial.SetInt("Boolean_8A146678", 1);
        }
        else
        {
            playerMaterial.SetInt("Boolean_8A146678", 0);
        }

        if ( leftFootCalc > .9f )
        {
            UpdateLeftFootPosition();
        }

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);

        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);

        if (rightFootWeight > .5)
        {
            playerMaterial.SetInt("Boolean_6D8D6C42", 1);
        }
        else
        {
            playerMaterial.SetInt("Boolean_6D8D6C42", 0);
        }

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

        if ( rightFootCalc > .9f )
        {
            UpdateRightFootPosition();
        }

        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);

        UpdateFigureHeight();
    }


    private void OnDrawGizmos()
    {
        //Ray gizmoRay = rightFootRay;
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(gizmoRay);
    }

    private void UpdateLeftFootPosition()
    {
        RaycastHit hit;
        Ray toGround = new Ray(leftFootBone.transform.position + legLength * Vector3.up + new Vector3(0,10f,0), Vector3.down); //animator.GetIKPosition(AvatarIKGoal.LeftFoot)
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength + 10f, enviroLayer))
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
        rightFootRay = new Ray(rightFootBone.transform.position + legLength * Vector3.up + new Vector3(0, 10f, 0), Vector3.down); //animator.GetIKPosition(AvatarIKGoal.RightFoot)
        if (Physics.Raycast(rightFootRay, out hit, distanceToGround + legLength + 10f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            rightLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            rightFootPosition = footPosition;
            rightFootRotation = Quaternion.LookRotation(transform.forward, hit.normal);
            Debug.Log(rightFootPosition);
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
}
