using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlemanBoss : MonoBehaviour
{
    private const string UprightPose = "UprightPose";
    private const string DownedPose = "DownedPose";
    private const string Death = "Death";
    private const string Expel = "Expel";
    private const string Defense = "Defense";
    private const string CurvedPose = "CurvedPose";

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

    public void PlayDeathPose()
    {
        animator.SetTrigger(Death);
    }
    public void PlayExpelPose()
    {
        animator.SetTrigger(Expel);
    }
    public void PlayDefensePose()
    {
        animator.SetTrigger(Defense);
    }
    public void PlayCurvedPose()
    {
        animator.SetTrigger(CurvedPose);
    }
}
