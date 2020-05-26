using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurnThePlayer : MonoBehaviour
{
    public WorldScroller worldScroller;
    public ParticleSystem[] effectsForBurningPlayer;
    struct DelayedEffectInstanciation
    {
        Vector3 position;
        float timeToSpawn;
    }
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
        if (pac != null && worldScroller != null)
        {
            worldScroller.PlayerIsBurning(5);
            // spawn effects on player that expire after a set time

            int numEffects = (int)((Random.value * 3) + 1);

            for(int i=0; i<numEffects; i++)
            {
                int effectIndex = (int)(Random.value * effectsForBurningPlayer.Length);

                //effectsForBurningPlayer
            }
            
        }
    }
}
