using UnityEngine;

public struct InputDataStruct
{
    public float[] axesTab;
    public bool[] buttonsTab;

    public InputDataStruct(int _axisNumber, int _buttonsNumber)
    {
        axesTab = new float[_axisNumber];
        buttonsTab = new bool[_buttonsNumber];
    }

    public void Reset()
    {
        for (int i = 0; i < axesTab.Length; i++)
        {
            axesTab[i] = 0f;
        }

        for (int i = 0; i < buttonsTab.Length; i++)
        {
            buttonsTab[i] = false;
        }
    }
}

public class InputManager : MonoBehaviour
{
    public static InputManager _inputMangerInstance;

    void Awake()
    {
        _inputMangerInstance = this;
    }

    [Range(0, 5)]
    public int AxisNumber;
    [Range(0, 10)]
    public int ButtonsNumber;

    public Controller controller;

    public void PassInputData(InputDataStruct dataInput)
    {
        //Debug.Log("Mov : "+data.axes[0] + " : " + data.axes[1]);
        controller.ReadInput(dataInput);
    }

    public void TrackerRefresh()
    {
        DeviceTracker _deviceTracker = GetComponent<DeviceTracker>();
        if (_deviceTracker != null)
        {
            _deviceTracker.Refresh();
        }
    }

}

