using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    //randomly spawns a platform to boost the player- will spawn 50 meters in front of player at random heights -cc
    public DistanceToEnd distanceTracker;
    public GameObject platformObj;

    public float minX, maxX;
    public float minY, maxY;
    void Start()
    {
        minX = 50;
        minY = 0;
        maxY = 100;
    }
    void Update(){
        float currentDis = distanceTracker.distance * -1;
        maxX = minX + 100;
        if (currentDis >= minX){
            minX += 50;
            Instantiate(platformObj, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), platformObj.transform.rotation);
        }
    }
}
