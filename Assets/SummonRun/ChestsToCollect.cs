﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChestsToCollect : MonoBehaviour
{
    public WorldScroller gcm;
    public GameObject[] rewardPrefabs;

    public GameObject[] obstaclePrefabs;
    public float currentDifficulty = 1;
    public Material[] matchMaterials;
    public GameObject obstacleContainer;

    public bool shouldGenerateObstacles = true;

    void Start()
    {
        Debug.Assert(gcm != null);
        HidePrefabs();
    }
    void HidePrefabs()
    {
        foreach (var p in rewardPrefabs)
        {
            p.SetActive(false);
        }
    }

    public void ChunkAdded(GameObject chunk)
    {
        if (shouldGenerateObstacles == false)
            return;

        Vector3 scale = chunk.GetComponent<MeshCollider>().bounds.size;
        NewChunkWasGenerated(chunk.transform, scale.x / 2, scale.z / 2);
    }
    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    void NewChunkWasGenerated(Transform chunkTransform, float boundsX, float boundsZ)
    {
        float rangeMin = currentDifficulty * 3;
        float rangeMax = currentDifficulty * 5;

        if (rangeMax > 100)
            rangeMax = 100;
        float numObstaclesToGenerate = UnityEngine.Random.Range(rangeMin, rangeMax);
        var center = chunkTransform.position;

        float margin = 0.5f;
        float positionMinX = center.x - boundsX + margin;
        float positionMaxX = center.x + boundsX - margin;
        float positionMinZ = center.z - boundsZ;
        float positionMaxZ = center.z + boundsZ;
        GameObject container = GetChildWithName(chunkTransform.gameObject, "ObjectsContainer");

        for (int i = 0; i < numObstaclesToGenerate; i++)
        {
            int whichReward = i % rewardPrefabs.Length;
            int whichObstacle = i% obstaclePrefabs.Length;
            //float height = Random.RandomRange(1, 25);
            float height = 0.68f;
            float angle = UnityEngine.Random.Range(0, 85);

            Quaternion q = rewardPrefabs[whichReward].transform.rotation;
            q *= Quaternion.Euler(Vector3.up * angle);
            float x = 0;// UnityEngine.Random.Range(positionMinX, positionMaxX);
            float z = UnityEngine.Random.Range(positionMinZ, positionMaxZ);
            Vector3 pos = new Vector3(x, height, z);

            
            CreateObstacle(whichReward, whichObstacle, container.transform, pos, q);
        }
    }

    GameObject CreateObstacle(int whichReward, int whichObstacle, Transform parent, Vector3 pos, Quaternion rotation)
    {
        //Vector3 scale = chunk.GetComponent<MeshCollider>().bounds.size;
       /* float obstacleHeight = obstaclePrefabs[whichObstacle].GetComponent<Collider>().bounds.size.z;
        float rewardHeight = rewardPrefabs[whichReward].GetComponent<Collider>().bounds.size.z;
        Vector3 obstaclePos = pos;
        obstaclePos.z += obstacleHeight / 2;
        pos.y += obstacleHeight + rewardHeight/2;*/
        GameObject newObstacle = Instantiate(obstaclePrefabs[whichObstacle], pos, rotation);
        Vector3 obstacleDimensions = newObstacle.GetComponent<Renderer>().bounds.size;
        //newObstacle.GetComponent<Renderer>().bounds.extents.
        //newObstacle.transform.position += new Vector3(0, obstacleDimensions.y/2, 0);

        GameObject newReward = Instantiate(rewardPrefabs[whichReward], pos, rotation);
        Vector3 rewardDimensions = newObstacle.GetComponent<Renderer>().bounds.size;
        Vector3 rewardPos = newReward.transform.position;
        rewardPos.y = obstacleDimensions.y;// + rewardDimensions.y / 2;
        newReward.transform.position = rewardPos;

        /* ClickableObstacle obst = newObstacle.GetComponent<ClickableObstacle>();
         if (obst)
         {
             obst.obstacleManager = this;
             obst.RandomizeIndex();
         }*/
        newObstacle.SetActive(true);
        newReward.SetActive(true);

        newObstacle.transform.parent = parent;
        newReward.transform.parent = parent;

        return newObstacle;
    }
}