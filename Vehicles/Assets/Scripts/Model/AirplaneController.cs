using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AirplaneController : Controller, IInteractive
{
    [SerializeField] Text _playerStateInfo;
    [SerializeField] Text _keysInfo;
    [SerializeField] private float horizontalSpeed = 3f;
    [SerializeField] private float verticalSpeed = 2.5f;
    [SerializeField] private GameObject pilotAvatar;
    
    private Vector3 flyVelocity;
    private Vector3 prevFlyVelocity;
    private float actualTurningAngel;
    private float turningAngelValuePerSecond = 90f;
    private Controller exitController;

    protected override void Start()
    {
        base.Start();

        pilotAvatar.SetActive(false);
        gameObject.layer = 10;
    }

    public override void ReadInput(InputDataStruct data)
    {
        prevFlyVelocity = flyVelocity;
        ResetMovementToZero();

        if (data.axesTab[1] != 0)
        {
            actualTurningAngel = turningAngelValuePerSecond * Time.deltaTime * (data.axesTab[1] > 0 ? 1 : -1);

            if (data.axesTab[1] > 0)
            {
                _playerStateInfo.text = "FLY - TURN RIGHT";
            }
            else if (data.axesTab[1] < 0)
            {
                _playerStateInfo.text = "FLY - TURN LEFT";
            }
        }
        //vertical movement
        if (data.axesTab[0] != 0f)
        {
            flyVelocity += Vector3.forward * data.axesTab[0] * horizontalSpeed;
            _playerStateInfo.text = "FLY - FORWARD";
        }

        //horizontal movement
        if (data.axesTab[1] != 0f)
        {
            flyVelocity += Vector3.right * data.axesTab[1] * horizontalSpeed;
        }

        //increase plane's flight altitude
        if (data.buttonsTab[0] == true)
        {
            flyVelocity += Vector3.up * verticalSpeed;
            _playerStateInfo.text = "FLY - UP";
        }

        //decrease plane's flight altitude
        if (data.buttonsTab[1] == true)
        {
            flyVelocity += Vector3.up * -verticalSpeed * 1.5f;
            _playerStateInfo.text = "FLY - DOWN";
        }

        //check exit from airplane
        if (data.buttonsTab[2] == true)
        {
            PilotExit();
        }

        newInput = true;
    }

    void LateUpdate()
    {
        if (!newInput)
        {
            prevFlyVelocity = flyVelocity;
            ResetMovementToZero();
        }
        if (prevFlyVelocity != flyVelocity)
        {
            //check if there is a face change
            //CheckForFacingChange();
        }
        _rigidBody.rotation = Quaternion.Euler(_rigidBody.rotation.eulerAngles + new Vector3(0, actualTurningAngel, 0));

        _rigidBody.velocity = flyVelocity;
        actualTurningAngel = 0f;
        newInput = false;
    }

    void ResetMovementToZero()
    {
        flyVelocity = Vector3.zero;
    }

    void PilotExit()
    {
        exitController.Activate();
    }

    public override void Activate()
    {
        exitController = InputManager._inputMangerInstance.controller;
        GetComponent<Rigidbody>().useGravity = false;
    
        base.Activate();
        pilotAvatar.SetActive(true);
        _keysInfo.text = "PLAYER FLYING:\n" + "W - forward\n" + "S - backward\n" + "A - right\n" + "D - left\n" +"SPACE - up\n" +"Z - down in\n" + "X - out from airplane\n";
    }

    public override void Deactivate()
    {
        GetComponent<Rigidbody>().useGravity = true;
        StopColliderForExitTime();
        base.Deactivate();
        //Destroy(this);
        pilotAvatar.SetActive(false);
        _keysInfo.text = "PLAYER WALKING:\n" + "W - forward\n" + "S - backward\n" + "A - right\n" + "D - left\n" + "SPACE - jump\n" + "Z - car on airplane in\n" + "X - attack\n"+"F - fire ball when looking forward";
    }
    public void OnInteract()
    {
        Activate();
    }

    IEnumerator StopColliderForExitTime()
    {
        GetComponent<Collider>().enabled = false;
        yield return null;
        GetComponent<Collider>().enabled = true;
    }
}
