using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChestsToCollect : MonoBehaviour
{
    public WorldScroller gcm;

    [Header("Prefabs")]
    public GameObject[] rewardPrefabs;
    public GameObject[] obstaclePrefabs;
    public Material[] matchMaterials;
    public GameObject obstacleContainer;

    [Header("settings and references")]
    [SerializeField]
    ParticleSystem collectCelebration;
    public PlayerAnimController playerAnimController;
    public float currentDifficulty = 1;
    public int score = 0;

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
        foreach (var p in obstaclePrefabs)
        {
            p.SetActive(false);
        }
        if (collectCelebration)
            collectCelebration.gameObject.SetActive(false);
    }

    public void ChunkAdded(GameObject chunk, bool isFirst = false)
    {
        if (shouldGenerateObstacles == false || isFirst == true)
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
       /* float positionMinX = center.x - boundsX + margin;
        float positionMaxX = center.x + boundsX - margin;*/
        float positionMinZ = center.z - boundsZ;
        float positionMaxZ = center.z + boundsZ;
        GameObject container = GetChildWithName(chunkTransform.gameObject, "ObjectsContainer");

        float oneTenth = (positionMaxZ - positionMinZ) / 10;
        float startingPosition = positionMinZ + oneTenth;
        for (int i = 0; i < numObstaclesToGenerate; i++)
        {
            int whichReward = i % rewardPrefabs.Length;
            int whichObstacle = i% obstaclePrefabs.Length;
            float angle = UnityEngine.Random.Range(0, 85);
            Quaternion q = rewardPrefabs[whichReward].transform.rotation;
            q *= Quaternion.Euler(Vector3.up * angle);

            float x = 0;// UnityEngine.Random.Range(positionMinX, positionMaxX);
            float z = UnityEngine.Random.Range(positionMinZ, positionMaxZ);

            
            CreateObstacle(whichReward, whichObstacle, container.transform, x, z, q);
        }
    }

    GameObject CreateObstacle(int whichReward, int whichObstacle, Transform parent, float x, float z, Quaternion rotation)
    {
        //Vector3 scale = obstaclePrefabs[whichObstacle].transform.localScale;
        Vector3 obstacleDimensions = obstaclePrefabs[whichObstacle].GetComponent<Renderer>().bounds.size;
        Vector3 pos2 = new Vector3(x, obstacleDimensions.y / 2, z);
        GameObject newObstacle = Instantiate(obstaclePrefabs[whichObstacle], pos2, rotation);


        //Vector3 rewardSCale = rewardPrefabs[whichReward].transform.localScale;
        Vector3 rewardDimensions = rewardPrefabs[whichReward].GetComponent<BoxCollider>().bounds.size;
        Vector3 rewardPos = new Vector3(x, obstacleDimensions.y + rewardDimensions.y / 2, z);

        GameObject newReward = Instantiate(rewardPrefabs[whichReward], rewardPos, rotation);
        var prize = newReward.GetComponent<CollectablePrize>();
        if(prize != null)
        {
            prize.chestManager = this;
        }


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

    public void ChestCollected(CollectablePrize collectablePrize)
    {
        collectablePrize.gameObject.SetActive(false);// play effect
        ParticleSystem ps = Instantiate(collectCelebration, collectablePrize.transform.position, Quaternion.identity);
        
        ps.transform.parent = collectablePrize.transform.parent;// this is the level
        ps.gameObject.SetActive(true);
        Destroy(ps, 3);

        playerAnimController.SpeedUp(0.5f);
        score++;
    }
}
