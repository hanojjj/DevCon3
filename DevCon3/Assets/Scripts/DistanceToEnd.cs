using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceToEnd : MonoBehaviour
{
    [SerializeField]
    public Transform checkpoint;
    public GameObject bullet;

    [SerializeField]
    public TextMeshProUGUI distanceText;

    public float distance;

    void Update()
    {

        if (bullet !=  null)
        {
            distance = (checkpoint.transform.position.x - bullet.transform.position.x);

            distanceText.text = "Distance: " + distance.ToString("F1") + " meters";
        }

    }
}
