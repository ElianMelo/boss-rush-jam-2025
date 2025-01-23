using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlemanBoss : MonoBehaviour
{
    private const string UprightPose = "UprightPose";
    private const string DownedPose = "DownedPose";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayUprightPose()
    {
        animator.SetTrigger(UprightPose);
    }

    public void PlayDownedPose()
    {
        animator.SetTrigger(DownedPose);
    }
}
