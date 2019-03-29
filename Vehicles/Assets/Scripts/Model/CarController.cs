using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CarController : Controller, IInteractive
{
    [SerializeField] Text _playerStateInfo;
    [SerializeField] Text _keysInfo;
    //Vehicle stats
    [SerializeField] private float carMaximumSpeed = 4f;
    [SerializeField] private float zeroToMaxSpeed = 2f;
    [SerializeField] private float maxToZeroSpeed = 5f;
    [SerializeField] private float brakingTime = 1f;
    [SerializeField] private float turningAngleValuePerSecond = 90f;
    [SerializeField] private float distanceToExit = 3f;
    [SerializeField] private GameObject driverAvatar;
    
    //current vehicle state
    float forwardSpeed;
    float actualTurning;
    bool accelerationChange;
    bool isBackward;

    //stats calculated at runtime
    float accelerationPerSecond;
    float slowingDownPersecond;
    float brakingRatePerSecond;

    //vehicle passenger information
    Controller exitController;

    protected override void Start()
    {
        base.Start();
        accelerationPerSecond = carMaximumSpeed / zeroToMaxSpeed;
        slowingDownPersecond = -carMaximumSpeed / maxToZeroSpeed;
        brakingRatePerSecond = -carMaximumSpeed / brakingTime;
        forwardSpeed = 0f;
        actualTurning = 0f;
        isBackward = false;
        driverAvatar.SetActive(false);
        gameObject.layer = 10;
    }

    public override void ReadInput(InputDataStruct data)
    {
        //turning
        if (data.axesTab[1] != 0)
        {
            actualTurning = turningAngleValuePerSecond * Time.deltaTime * (data.axesTab[1] > 0 ? 1 : -1);

            if (data.axesTab[1] > 0)
            {
                _playerStateInfo.text = "CAR - TURN RIGHT";
            }
            else if (data.axesTab[1] < 0)
            {
                _playerStateInfo.text = "CAR - TURN LEFT";
            }
        }

        //moving backward
        if (data.axesTab[0] < 0)
        {
            isBackward = true;
            _playerStateInfo.text = "CAR - BACKWARD";
        }

        //acceleration
        if (data.buttonsTab[0] == true)
        {
            CarAcceleration(accelerationPerSecond);
            _playerStateInfo.text = "CAR - FORWARD";
        }

        //braking
        if (data.buttonsTab[1] == true)
        {
            CarStoping(brakingRatePerSecond);
            _playerStateInfo.text = "CAR - STOP";
        }

        //exit
        if (data.buttonsTab[2] == true)
        {
            _playerStateInfo.text = "CAR - EXIT";
            GetOutFromCar();
        }

        newInput = true;
    }

    void LateUpdate()
    {
        //if moving, turn vehicle
        if (forwardSpeed != 0f)
        {
            _rigidBody.rotation = Quaternion.Euler(_rigidBody.rotation.eulerAngles + new Vector3(0, actualTurning, 0));
        }

        if (!accelerationChange)
        {
            CarStoping(slowingDownPersecond);
        }

        //move vehicle based on actual forward speed
        _rigidBody.velocity = transform.forward * forwardSpeed;

        //reset for next frame
        accelerationChange = false;
        actualTurning = 0f;
        isBackward = false;
        newInput = false;
    }

    void CarAcceleration(float accel)
    {
        float reverseFactor = isBackward ? -1 : 1;
        forwardSpeed += accel * Time.deltaTime * reverseFactor;
        forwardSpeed = Mathf.Clamp(forwardSpeed, -carMaximumSpeed, carMaximumSpeed);
        accelerationChange = true;
    }

    void CarStoping(float decel)
    {
        if (forwardSpeed == 0)
        {
            accelerationChange = true;
            return;
        }
        float reverseFactor = Mathf.Sign(forwardSpeed);
        forwardSpeed = Mathf.Abs(forwardSpeed);
        forwardSpeed += decel * Time.deltaTime;
        forwardSpeed = Mathf.Max(forwardSpeed, 0) * reverseFactor;
        accelerationChange = true;
    }

    public override void Activate()
    {
        exitController = InputManager._inputMangerInstance.controller;
        base.Activate();
        driverAvatar.SetActive(true);
        _keysInfo.text = "PLAYER DRIVING:\n" + "W - forward\n" + "S - backward\n" + "A - right\n" + "D - left\n" + "SPACE - engine accelerator\n" + "Z - break in\n" + "X - out from car\n";
    }

    public override void Deactivate()
    {
        //deactivate the collider for one frame
        StopColliderWhenDriverLeftCar();
        base.Deactivate();
        driverAvatar.SetActive(false);
        _keysInfo.text = "PLAYER WALKING:\n" + "W - forward\n" + "S - backward\n"+ "A - right\n" + "D - left\n" + "SPACE - jump\n" + "Z - car on airplane in\n" + "X - attack\n" + "F - fire ball when looking forward";
    }

    void GetOutFromCar()
    {
        if (DriverExitPointCheck())
        {
            exitController.Activate();
        }
    }
    public void OnInteract()
    {
        Activate();
    }

    bool DriverExitPointCheck()
    {
        Vector3 exitForDriver = transform.position + transform.right * -distanceToExit;
        if (Physics.OverlapBox(exitForDriver + Vector3.up, Vector3.one * 0.5f).Length > 0)
        {
            Debug.Log("Obstacle in the way!");
            _playerStateInfo.text = "DRIVE SOMWHERE ELSE YOU CAN NOT EXIT HERE";
            return false;
        }
        exitController.transform.position = exitForDriver + Vector3.up;
        return true;
    }

    IEnumerator StopColliderWhenDriverLeftCar()
    {
        GetComponent<Collider>().enabled = false;
        yield return null;
        GetComponent<Collider>().enabled = true;
    }

}
