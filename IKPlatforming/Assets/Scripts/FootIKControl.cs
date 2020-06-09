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

    private float leftFootWeight;
    private float rightFootWeight;
    private float leftFootCalc;
    private float rightFootCalc;

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
        leftFootWeight = animator.GetFloat("IKLeftFootWeight");
        rightFootWeight = animator.GetFloat("IKRightFootWeight");

        leftFootCalc = animator.GetFloat("IKLeftFootCalc");
        rightFootCalc = animator.GetFloat("IKRightFootCalc");

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);

        ResetFigureHeight();

        if ( leftFootCalc > .1f )
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

        if ( rightFootCalc > .1f )
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
        Ray toGround = new Ray(leftFootBone.transform.position + legLength * Vector3.up + new Vector3(0,10f,0), Vector3.down); //animator.GetIKPosition(AvatarIKGoal.LeftFoot)
        if (Physics.Raycast(toGround, out hit, distanceToGround + legLength + 10f, enviroLayer))
        {
            Vector3 footPosition = hit.point;
            leftLegLength = transform.parent.transform.position.y - footPosition.y + capsuleBottomHeight;
            footPosition.y += distanceToGround;

            // Shamelessly stolen from https://answers.unity.com/questions/1591763/how-to-fix-the-rotation-of-my-ik-foot.html

            Vector3 rotAxis = Vector3.Cross(Vector3.up, hit.normal);
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            Quaternion rot = Quaternion.AngleAxis(angle * leftFootWeight, rotAxis);

            leftFootPosition = footPosition;
            leftFootRotation = rot * animator.GetIKRotation(AvatarIKGoal.LeftFoot);//Quaternion.LookRotation(transform.forward, hit.normal);
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

            Vector3 rotAxis = Vector3.Cross(Vector3.up, hit.normal);
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            Quaternion rot = Quaternion.AngleAxis(angle * rightFootWeight, rotAxis);

            rightFootPosition = footPosition;
            rightFootRotation = rot * animator.GetIKRotation(AvatarIKGoal.RightFoot);//Quaternion.LookRotation(transform.forward, hit.normal);
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
