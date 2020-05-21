using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundChunkManager : MonoBehaviour
{
    public ObstacleManager obstacleManager;

    public GameObject[] chunkModels;
    List<GameObject> chunkList;
    //public NavMeshSurface navMeshSurface;
    public int numModelsIntoDistance = 2;
    float playerZ = 0;
    float distanceBeforeCreatingNewChunk = 45;
    float chunkLength = 15;
    float lastDistanceChunk;
    Vector3 lastChunkPositionPlaced;
    // Start is called before the first frame update
    void Start()
    {
        chunkList = new List<GameObject>();
        
        SetupScene();
        HideAllWorkingChunks();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideAllWorkingChunks()
    {
        foreach (var i in chunkModels)
            i.SetActive(false);
    }
    void SetupScene()
    {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i <= numModelsIntoDistance; i++)
        {
            GameObject floor = AddChunkToWorld(position);
            chunkList.Add(floor);
            float length = floor.transform.lossyScale.z;

            //var collider = floor.GetComponent<Collider>();
           // var mesh = floor.GetComponent<Mesh>();
            //float test2 = collider.bounds.size.z;
            //float test = mesh.bounds.size.z;

            position.z += length;
        }
        distanceBeforeCreatingNewChunk = position.z;
        lastDistanceChunk = position.z;
        lastChunkPositionPlaced = position;

        chunkLength = chunkModels[0].transform.lossyScale.z;// todo, generalize this
        Debug.Log("chunk length: " + chunkLength);

        //RebuildNavMesh();
    }

    /*void RebuildNavMesh()
    {
        if (navMeshSurface == null)
            return;

        var settings = navMeshSurface.GetBuildSettings();

        settings.agentRadius = 0.5f;
        settings.agentHeight = 2.0f;
        settings.agentSlope = 23;
        settings.agentClimb = 0.4f;
        settings.minRegionArea = 2.0f;

        navMeshSurface.BuildNavMesh();
    }*/

    GameObject AddChunkToWorld(Vector3 position)
    {
        float which = Random.Range(0, chunkModels.Length);
        GameObject newChunk = Instantiate(chunkModels[(int)which], this.transform);
        newChunk.transform.position = position;
        newChunk.SetActive(true);
       /* if(obstacleManager != null)
        {
            obstacleManager.ChunkAdded(newChunk);
        }*/
        return newChunk;
    }

    void DeleteOldChunk()
    {
        GameObject go = chunkList[0];
        chunkList.RemoveAt(0);
        Destroy(go);
        //chunkList[chunkList.Count - 1]
    }

    public void UpdateWorldPosition(Vector3 playerPosition)
    {
        if(playerZ < playerPosition.z)
        {
            playerZ = playerPosition.z;
        }

        if (lastChunkPositionPlaced.z + chunkLength  < playerZ + distanceBeforeCreatingNewChunk)
        {
            Vector3 v = lastChunkPositionPlaced;
            //v.z += ;
            GameObject floor = AddChunkToWorld(v);
            chunkList.Add(floor);

            MoveTheLastChunkTrackingForward();
            DeleteOldChunk();
            //RebuildNavMesh();
        }
    }

    void MoveTheLastChunkTrackingForward()
    {
        lastChunkPositionPlaced = chunkList[chunkList.Count - 1].transform.position;
        lastChunkPositionPlaced.z += chunkLength;
    }
     
}
