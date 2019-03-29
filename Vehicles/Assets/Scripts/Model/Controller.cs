using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    //TODO: Add method ReadInput
    public abstract void ReadInput(InputDataStruct data);

    public bool onSceneWhenNotUsed = true;

    protected Rigidbody _rigidBody;
    protected Collider _collider;
    protected Camera _camera;
    protected bool newInput;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _camera = GetComponentInChildren<Camera>();
    }

    protected virtual void Start()
    {
        if (InputManager._inputMangerInstance.controller != this)
        {
            Deactivate();
        }
    }

    public virtual void Activate()
    {
        InputManager._inputMangerInstance.controller.Deactivate();
        InputManager._inputMangerInstance.controller = this;
        _camera.gameObject.SetActive(true);
        gameObject.SetActive(true);

    }

    public virtual void Deactivate()
    {
        _camera.gameObject.SetActive(false);
        if (!onSceneWhenNotUsed)
        {
            gameObject.SetActive(false);
        }
    }
}
