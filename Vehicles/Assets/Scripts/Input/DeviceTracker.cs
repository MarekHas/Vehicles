using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputManager))]
public abstract class DeviceTracker : MonoBehaviour
{
    protected InputManager _inputManager;
    protected InputDataStruct _inputData;
    protected bool isNewData;//tracking if is new data input in

    void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _inputData = new InputDataStruct(_inputManager.AxisNumber, _inputManager.ButtonsNumber);
    }
 
    public abstract void Refresh();
}
