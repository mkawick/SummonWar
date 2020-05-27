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
    PlayerAnimController player;
    bool areWeBurningThePlayer = false;
    float timeToReset = 0;
    Vector3 originalCameraPosition;
    Quaternion originalCameraRotation;
    Camera currentCamera;

    //-----------------------------------------------------------
    void Start()
    {
        pendingEffects = new List<DelayedEffectInstanciation>();
        foreach( var camera in Camera.allCameras)
        {
            if(camera.isActiveAndEnabled == true)
            {
                currentCamera = camera;
                break;
            }
        }
        originalCameraPosition = currentCamera.transform.position;
        originalCameraRotation = currentCamera.transform.rotation;
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

                currentCamera.transform.position = originalCameraPosition;
                currentCamera.transform.rotation = originalCameraRotation;
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
                var newEffect = Instantiate(effectsForBurningPlayer[effect.effectIndex], effect.position, Quaternion.identity);
                pendingEffects.Remove(effect);
                float length = timeToReset - Time.time;
                Destroy(newEffect, length);
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

            player = pac;
            SetupCameraMovement(player.transform.position);
            AddDelayedEffectsToPlayer(player);
            player.Idle();
            var burn = other.GetComponent<BurningEffect>();
            burn.StartBurn();
        }
    }

    void AddDelayedEffectsToPlayer(PlayerAnimController pac)
    {
        int numEffects = (int)((UnityEngine.Random.value * numBurningEffectsToSpawn) + 1);

        for (int i = 0; i < numEffects; i++)
        {
            int effectIndex = (int)(UnityEngine.Random.value * effectsForBurningPlayer.Length);
            Vector3 pos = pac.transform.position;
            pos.y = UnityEngine.Random.value * 3;// magic number
            float delay = UnityEngine.Random.value * (howLongDoesPlayerBurn / 2);

            DelayedEffectInstanciation dei = new DelayedEffectInstanciation();
            dei.position = pos;
            dei.timeToSpawn = Time.time + delay;
            dei.effectIndex = effectIndex;

            pendingEffects.Add(dei);
        }
    }

    void SetupCameraMovement(Vector3 moveTowardPosition)
    {
        Vector3 ray = (moveTowardPosition - currentCamera.transform.position);
        float len = ray.magnitude;
        ray.Normalize();
        ray *= (len * 0.8f);
        Vector3 destination = currentCamera.transform.position + ray;
        float zoomFaster = howLongDoesPlayerBurn * 0.9f;
        iTween.MoveTo(currentCamera.gameObject, destination, zoomFaster);
    }
}
