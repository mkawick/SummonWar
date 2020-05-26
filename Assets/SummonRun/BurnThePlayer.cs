using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurnThePlayer : MonoBehaviour
{
    public WorldScroller worldScroller;
    public ParticleSystem[] effectsForBurningPlayer;
    public float howLongDoesPlayerBurn = 5;
    public float numBurningEffectsToSpawn = 3;
    struct DelayedEffectInstanciation
    {
        public Vector3 position;
        public float timeToSpawn;
        public int effectIndex;
    }

    List<DelayedEffectInstanciation> pendingEffects;
    bool areWeBurningThePlayer = false;
    float timeToReset = 0;

    //-----------------------------------------------------------
    void Start()
    {
        pendingEffects = new List<DelayedEffectInstanciation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(areWeBurningThePlayer)
        {
            if(Time.time > timeToReset)
            {
                timeToReset = 0;
                pendingEffects = new List<DelayedEffectInstanciation>();
                areWeBurningThePlayer = false;
            }
            else
            {
                InstantiateEffects();
            }
        }
    }

    private void InstantiateEffects()
    {
        foreach(var effect in pendingEffects)
        {
            if(effect.timeToSpawn < Time.time)
            {
                Instantiate(effectsForBurningPlayer[effect.effectIndex], effect.position, Quaternion.identity);
                pendingEffects.Remove(effect);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var pac = other.GetComponent<PlayerAnimController>();
        if (pac != null && worldScroller != null)
        {
            timeToReset = Time.time + howLongDoesPlayerBurn;
            areWeBurningThePlayer = true;
            worldScroller.PlayerIsBurning(howLongDoesPlayerBurn);
            // spawn effects on player that expire after a set time

            AddDelayedEffectsToPlayer(pac);
        }
    }

    void AddDelayedEffectsToPlayer(PlayerAnimController pac)
    {
        int numEffects = (int)((UnityEngine.Random.value * numBurningEffectsToSpawn) + 1);

        for (int i = 0; i < numEffects; i++)
        {
            int effectIndex = (int)(UnityEngine.Random.value * effectsForBurningPlayer.Length);
            Vector3 pos = pac.transform.position;
            pos.y = UnityEngine.Random.value * 2;// magic number
            float delay = UnityEngine.Random.value * (howLongDoesPlayerBurn / 2);

            DelayedEffectInstanciation dei = new DelayedEffectInstanciation();
            dei.position = pos;
            dei.timeToSpawn = Time.time + delay;
            dei.effectIndex = effectIndex;

            pendingEffects.Add(dei);
        }
    }
}
