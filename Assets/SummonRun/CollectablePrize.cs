using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePrize : MonoBehaviour
{
    public ChestsToCollect chestManager { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        var pac = other.GetComponent<PlayerAnimController>();
        if(pac != null)
        {
            if(chestManager != null)
            {
                chestManager.ChestCollected(this);
            }
        }
    }
}
