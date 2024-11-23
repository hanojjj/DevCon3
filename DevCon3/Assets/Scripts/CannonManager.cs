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
    [SerializeField] public GameObject BarrelAnchorPivot;
    [SerializeField] public GameObject FirePoint;
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
    private void GatherInputs()
    {
        //fire button logic
        if (Input.GetKeyDown(fireButton))
        {
            weShouldDoFiringLogic = true;
        }
    }

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

    //i dont think i really need this function but whatever
    private void UpdateSliderToMinigameValue(Slider selectedSlider)
    {
        selectedSlider.value = miniGameCounter;
    }
}
