using UnityEngine;

public class LookingDirection : MonoBehaviour {

    public float offset = 0.5f;

    void Awake()
    {
        WalkingController.OnLookingChange += RefreshFacing;
    }

    public void RefreshFacing(LookingDirectionName fd)
    {
        switch (fd)
        {
            case LookingDirectionName.FORWARD:
                transform.localPosition = Vector3.forward * offset;
                break;
            case LookingDirectionName.RIGHT:
                transform.localPosition = Vector3.right * offset;
                break;
            case LookingDirectionName.LEFT:
                transform.localPosition = Vector3.left * offset;
                break;
            default:
                transform.localPosition = Vector3.back * offset;
                break;

        }
    }
}
