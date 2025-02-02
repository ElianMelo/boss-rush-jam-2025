using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRootMaterial : MonoBehaviour
{
    public GameObject targetObject;
    public Material newMaterial;
    
    public void SwapRootMaterial()
    {
        targetObject.GetComponent<SkinnedMeshRenderer>().material = newMaterial;
    }
}
