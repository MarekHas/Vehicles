using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AxisKeys
{
    public KeyCode positiveKey;//axis[0] up, axis[1] right
    public KeyCode negativeKey;//axis[0] down, axis[1] left`
}

public class KeyboardTracker : DeviceTracker
{
    public AxisKeys[] AxisKeysArray;//struct keycode negative postive
    public KeyCode[] ButtonKeysArray;
    
    void Reset()
    {
        _inputManager = GetComponent<InputManager>();
        AxisKeysArray = new AxisKeys[_inputManager.AxisNumber];
        ButtonKeysArray = new KeyCode[_inputManager.ButtonsNumber];
    }

    public override void Refresh()
    {
        _inputManager = GetComponent<InputManager>();

        //create 2 temp arrays for buttons and axes
        KeyCode[] newButtonsArray = new KeyCode[_inputManager.ButtonsNumber];
        AxisKeys[] newAxesArray = new AxisKeys[_inputManager.AxisNumber];

        if (ButtonKeysArray != null)
        {
            for (int i = 0; i < Mathf.Min(newButtonsArray.Length, ButtonKeysArray.Length); i++)
            {
                newButtonsArray[i] = ButtonKeysArray[i];
            }
        }
        ButtonKeysArray = newButtonsArray;

        if (AxisKeysArray != null)
        {
            for (int i = 0; i < Mathf.Min(newAxesArray.Length, AxisKeysArray.Length); i++)
            {
                newAxesArray[i] = AxisKeysArray[i];
            }
        }
        AxisKeysArray = newAxesArray;
    }

    void Update()
    {
        //when inputs detected set isNewData to true
        for (int i = 0; i < AxisKeysArray.Length; i++)
        {
            float val = 0f;
            if (Input.GetKey(AxisKeysArray[i].positiveKey))
            {
                val += 1f;
                isNewData = true;
            }
            if (Input.GetKey(AxisKeysArray[i].negativeKey))
            {
                val -= 1f;
                isNewData = true;
            }
            _inputData.axesTab[i] = val;
        }

        for (int i = 0; i < ButtonKeysArray.Length; i++)
        {
            if (Input.GetKey(ButtonKeysArray[i]))
            {
                _inputData.buttonsTab[i] = true;
                isNewData = true;
            }
        }

        if (isNewData)
        {
            _inputManager.PassInputData(_inputData);
            isNewData = false;
            _inputData.Reset();//Input manager
        }
    }
}
