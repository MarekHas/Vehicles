using UnityEngine;
using System.Collections;

public enum ActionType
{
    Interact,
    Attack
}

[RequireComponent(typeof(BoxCollider))]
public class PlayerGrasper : MonoBehaviour
{

    //collider movement
    private float offset = 1f;
    private BoxCollider _collider;

    //collider duration
    private float collisonTime;

    //secondary float value
    private float secondary;

    //track action type
    ActionType action;

    void Awake()
    {
        WalkingController.OnLookingChange += RefreshFacing;
        WalkingController.OnInteraction += CollisionChecking;
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        gameObject.layer = 9;
    }

    void Update()
    {
        if (_collider.enabled)
        {
            collisonTime -= Time.deltaTime;
            if (collisonTime <= 0)
            {
                _collider.enabled = false;
            }
        }
    }

    void CollisionChecking(float _time, float _sec, ActionType _actionTypeName)
    {
        action = _actionTypeName;
        _collider.enabled = true;
        collisonTime = _time;
        secondary = _sec;
    }

    void RefreshFacing(LookingDirectionName fd)
    {
        switch (fd)
        {
            case LookingDirectionName.FORWARD:
                _collider.center = Vector3.forward * offset;
                break;
            case LookingDirectionName.RIGHT:
                _collider.center = Vector3.right * offset;
                break;
            case LookingDirectionName.LEFT:
                _collider.center = Vector3.left * offset;
                break;
            default:
                _collider.center = Vector3.back * offset;
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (action == ActionType.Interact && col.GetComponent<IInteractive>() != null)
        {
            col.GetComponent<IInteractive>().OnInteract();
        }
        else if (col.GetComponent<ICanBeAttacked>() != null)
        {
            col.GetComponent<ICanBeAttacked>().ReciveDamage(secondary);
        }

    }

}
