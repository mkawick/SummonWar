using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScroller : MonoBehaviour
{
    //GameObject scrollingTrack;
    public GameObject[] chunkModels;
    List<GameObject> chunkList;
    //public NavMeshSurface navMeshSurface;
    public int numModelsIntoDistance = 2;
    float playerZ = 0;
    float distanceBeforeCreatingNewChunk = 45;
    float chunkLength = 15;
    Vector3 lastChunkPositionPlaced;

    public ChestsToCollect chestMaker;
    public bool shouldScroll = true;
    public bool shouldFlamesScroll = true;
    bool waitingToReset = false;
    float resetTimeout = 0;

    [Range(1, 16)]
    public float scrollSpeed = 2.5f;

    [Range(1, 5)]
    public float flameScrollSpeed = 2.5f;

    public Transform flameCube;
    public Vector3 flameCubeOriginalPosition;

    void Start()
    {
        Reset();
        flameCubeOriginalPosition = flameCube.position;
    }

    private void Reset()
    {
        chunkList = new List<GameObject>();
        SetupScene();
        HideAllWorkingChunks();
        flameCube.position = flameCubeOriginalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingToReset == true)
        {
            if(resetTimeout < Time.time)
            {
                Reset();
                waitingToReset = false;
            }
        }
        else 
        {
            Scroll();
        }
        
    }

    void Scroll()
    {
        if(shouldScroll == true)
            transform.position -= new Vector3(0, 0, scrollSpeed * Time.deltaTime);

        if(shouldFlamesScroll == true)
        {
            if(flameCube != null)
            {
                flameCube.position += new Vector3(0, 0, flameScrollSpeed * Time.deltaTime);
            }
        }
    }
    void SetupScene()
    {
        Vector3 position = new Vector3(0, 0, -6);
        for (int i = 0; i <= numModelsIntoDistance; i++)
        {
            bool isFirst = i == 0;
            GameObject floor = AddChunkToWorld(position, isFirst);
            chunkList.Add(floor);
            Vector3 objectSize = Vector3.Scale(floor.transform.localScale, floor.GetComponent<MeshCollider>().bounds.size);
            float length = //floor.transform.lossyScale.z;
                floor.GetComponent<MeshCollider>().bounds.size.z;

            position.z += length;
        }
        distanceBeforeCreatingNewChunk = position.z;
        lastChunkPositionPlaced = position;

        chunkLength = chunkModels[0].transform.lossyScale.z;// todo, generalize this
        Debug.Log("chunk length: " + chunkLength);

        //RebuildNavMesh();
    }

    GameObject AddChunkToWorld(Vector3 position, bool isFirstChunk)
    {
        float which = Random.Range(0, chunkModels.Length);
        GameObject newChunk = Instantiate(chunkModels[(int)which], this.transform);
        newChunk.transform.position = position;
        newChunk.SetActive(true);
         if(chestMaker != null)
         {
             chestMaker.ChunkAdded(newChunk, isFirstChunk);
         }
        return newChunk;
    }
    void HideAllWorkingChunks()
    {
        foreach (var i in chunkModels)
            i.SetActive(false);
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
        if (playerZ < playerPosition.z)
        {
            playerZ = playerPosition.z;
        }

        if (lastChunkPositionPlaced.z + chunkLength < playerZ + distanceBeforeCreatingNewChunk)
        {
            Vector3 v = lastChunkPositionPlaced;
            //v.z += ;
            GameObject floor = AddChunkToWorld(v, false);
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

    public void PlayerIsBurning(float waitTimeForBurning)
    {
        shouldScroll = false;
        waitingToReset = true;
        resetTimeout = Time.time + waitTimeForBurning;
    }
}
