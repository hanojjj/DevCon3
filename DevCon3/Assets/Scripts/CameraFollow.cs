using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform bullet;
    CannonManager cannonManager;

    void Start()
    {
        cannonManager = GameObject.Find("Cannon").GetComponent<CannonManager>();
    }


    void Update()
    {
        if (cannonManager.hasFired == true)
        {
            bullet = GameObject.FindWithTag("Bullet").transform;
            transform.position = new Vector3(bullet.position.x, bullet.position.y, transform.position.z);
        }
        else { Debug.Log("FIRE, YOU JERK.");
        }
    }
}
