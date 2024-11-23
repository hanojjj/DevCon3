using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonManager : MonoBehaviour
{
    [Header("Minigame Properties")]
    [SerializeField] public GameObject AngleAdjustSliderObj;
    [SerializeField] public GameObject PowerAdjustSliderObj;
    [SerializeField] private float miniGameSpeed = 1;
    //hidden
    [HideInInspector] public Slider AngleAdjustSlider;
    [HideInInspector] public Slider PowerAdjustSlider;
    [HideInInspector] private float miniGameCounter = 0;
    [HideInInspector] private bool shouldScaleCountUp = true;

    [Header("Keybinds")]
    [SerializeField] public KeyCode fireButton = KeyCode.Space;

    [Header("Manager Properties")]
    [SerializeField] public Transform firePoint;
    [SerializeField] public GameObject ThingTheCannonFires;
    [SerializeField] public GameObject BarrelAnchorPivot;
    [SerializeField] public float maxCannonPower = 100;
    [SerializeField] public float currentCannonPower = 0;
    private FireState currentFireState;
    private bool weShouldDoFiringLogic = false;

    private enum FireState
    {
        WaitingForInput,
        AdjustingAngle,
        AdjustingPower,
        Fired
    }



    // Unity Functions
    private void Start()
    {
        currentFireState = FireState.WaitingForInput;
        AngleAdjustSlider = AngleAdjustSliderObj.GetComponent<Slider>();
        PowerAdjustSlider = PowerAdjustSliderObj.GetComponent<Slider>();
    }

    private void Update()
    {
        GatherInputs();
    }

    private void FixedUpdate()
    {
        //Firing logic
        if (weShouldDoFiringLogic)
        {
            switch (currentFireState) {
                case FireState.WaitingForInput:
                    //Start Adjusting Angle Minigame
                    AngleAdjustSliderObj.SetActive(true);
                    currentFireState = FireState.AdjustingAngle;
                    break;

                case FireState.AdjustingAngle:
                    //Final Update to slider (should be the same as last update but just in case)
                    UpdateSliderToMinigameValue(AngleAdjustSlider);
                    //Resets Counter
                    miniGameCounter = 0;
                    //Start Adjusting Power Minigame
                    PowerAdjustSliderObj.SetActive(true);
                    currentFireState = FireState.AdjustingPower;
                    break;

                case FireState.AdjustingPower:
                    //Final Update to slider (should be the same as last update but just in case)
                    UpdateSliderToMinigameValue(PowerAdjustSlider);
                    //Resets Counter
                    miniGameCounter = 0;
                    //Change State
                    currentFireState = FireState.Fired;

                    //FIRE CANNON HERE!!!
                    FireCannon();

                    break;
            }
            weShouldDoFiringLogic = false;
        }

        //Minigame Logic
        switch (currentFireState)
        {
            case FireState.AdjustingAngle:
                RunMinigameLogic();
                UpdateSliderToMinigameValue(AngleAdjustSlider);
                Quaternion cannonRotation = Quaternion.Euler(0, 0, -90 - (-90 * miniGameCounter));
                BarrelAnchorPivot.transform.rotation = cannonRotation;
                break;

            case FireState.AdjustingPower:
                RunMinigameLogic();
                UpdateSliderToMinigameValue(PowerAdjustSlider);
                currentCannonPower = maxCannonPower * miniGameCounter;
                break;
        }
    }








    // Script Specific Functions!
    /// <summary>
    /// Function that grabs from Input
    /// </summary>
    private void GatherInputs()
    {
        //fire button logic
        if (Input.GetKeyDown(fireButton))
        {
            weShouldDoFiringLogic = true;
        }
    }

    /// <summary>
    /// Moves miniGameCounter from 0 to 1
    /// </summary>
    private void RunMinigameLogic()
    {
        //sets direction
        if (miniGameCounter >= 1)
        {
            shouldScaleCountUp = false;
        } else if (miniGameCounter <= 0)
        {
            shouldScaleCountUp = true;
        }

        //counter goes up/down based on direction. (speed * deltaTime)/200 so the counter has simulated "200" points on it
        if (shouldScaleCountUp)
        {
            miniGameCounter += miniGameSpeed * 0.005f;
        }
        else
        {
            miniGameCounter -= miniGameSpeed * 0.005f;
        }

        //just in case, counter only stays between 1 and 0
        if (miniGameCounter > 1)
        {
            miniGameCounter = 1;
        } else if (miniGameCounter < 0)
        {
            miniGameCounter = 0;
        }
    }

    /// <summary>
    /// updates given slider to miniGameCounter value
    /// </summary>
    /// <param name="selectedSlider"></param>
    private void UpdateSliderToMinigameValue(Slider selectedSlider)
    {
        selectedSlider.value = miniGameCounter;
    }

    /// <summary>
    /// Instantiates our projectile, Adds a force based on cannon direction, and given power
    /// </summary>
    private void FireCannon()
    {
        // Creates Projectile
        Rigidbody2D cannonShot = Instantiate(ThingTheCannonFires).GetComponent<Rigidbody2D>();
        //sets transform values of our shot
        cannonShot.transform.position = firePoint.transform.position;
        cannonShot.transform.rotation = BarrelAnchorPivot.transform.rotation;

        //readable refrence to cannon angle
        float cannonAngle = BarrelAnchorPivot.transform.rotation.z;
        //Calculates force to be added to projectile
        //Sin and Cos return values from 0-1 based on the radian inputted. These X and Y values are multiplied by our cannon power, giving us a vector2 force
        Vector2 shotDirection = new Vector2(-Mathf.Sin(cannonAngle) * currentCannonPower, Mathf.Cos(cannonAngle) * currentCannonPower);
        //then we add force
        cannonShot.AddForce(shotDirection, ForceMode2D.Impulse);
    }
}
