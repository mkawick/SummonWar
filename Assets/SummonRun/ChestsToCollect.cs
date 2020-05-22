using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChestsToCollect : MonoBehaviour
{
    public WorldScroller gcm;
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
        foreach (var p in obstaclePrefabs)
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
        for (int i = 0; i < numObstaclesToGenerate; i++)
        {
            int whichObstacle = i % obstaclePrefabs.Length;
            //float height = Random.RandomRange(1, 25);
            float height = 0.68f;
            float angle = UnityEngine.Random.Range(0, 85);

            Quaternion q = obstaclePrefabs[whichObstacle].transform.rotation;
            q *= Quaternion.Euler(Vector3.up * angle);
            float x = UnityEngine.Random.Range(positionMinX, positionMaxX);
            float z = UnityEngine.Random.Range(positionMinZ, positionMaxZ);
            Vector3 pos = new Vector3(x, height, z);

            GameObject container = GetChildWithName(chunkTransform.gameObject, "ObjectsContainer");
            CreateObstacle(whichObstacle, pos, q).transform.parent = container.transform;
            // when we delete this chunk later, all of the obstacles will go too.
            //CreateObstacle(whichObstacle, pos, q).transform.parent = chunkTransform;
        }

    }
    GameObject CreateObstacle(int which, Vector3 pos, Quaternion q)
    {
        GameObject newObstacle = Instantiate(obstaclePrefabs[which], pos, q);
        /* ClickableObstacle obst = newObstacle.GetComponent<ClickableObstacle>();
         if (obst)
         {
             obst.obstacleManager = this;
             obst.RandomizeIndex();
         }*/
        newObstacle.SetActive(true);

        
        return newObstacle;
    }
}
