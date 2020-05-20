using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIKControl : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
