using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : MonoBehaviour
{
    public float burnDelay = 2;
    public float burnTime = 3;
    bool isBurning;
    float percentage = 0;
    float burnDelayStart = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(isBurning == true && burnDelayStart < Time.time)
        {
            Fade();
        }
    }

    public void StartBurn()
    {
        isBurning = true;
        burnDelayStart = Time.time + burnDelay;
    }
    private void Fade()
    {
        //percentage += 0.01f;
        percentage += Time.deltaTime / burnTime;
        if (percentage > 1)
        {
            percentage = 0;
            isBurning = false;
            // do not return. We need to restore to 0
        }

        foreach (var smr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            smr.sharedMaterial.SetFloat("Vector1_6FF01B72", percentage);
        }
        foreach (var smr in GetComponentsInChildren<MeshRenderer>())
        {
            smr.sharedMaterial.SetFloat("Vector1_6FF01B72", percentage);
        }
    }
}
