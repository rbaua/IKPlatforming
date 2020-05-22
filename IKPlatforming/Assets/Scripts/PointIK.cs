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

        Vector3 pointPosition = animator.GetIKPosition(AvatarIKGoal.RightHand) + (transform.forward * armLength) + (Input.mousePosition/100);
        //footPosition.y += distanceToGround;
        animator.SetIKPosition(AvatarIKGoal.RightHand, pointPosition);
    }
}
