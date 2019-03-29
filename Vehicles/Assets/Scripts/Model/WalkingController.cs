using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public enum LookingDirectionName
{
    FORWARD,
    RIGHT,
    BACKWARD,
    LEFT
}


public class WalkingController : Controller
{
    [SerializeField] private Text infoText;
    [SerializeField] private Text _keysInfo;
    //movement information
    Vector3 walkingVector;
    Vector3 previosWalkingVector;//previos walk velocity
    private LookingDirectionName playerLookingDirection = LookingDirectionName.FORWARD;

    public LookingDirectionName PlayerLookingDirection
    {
        get { return playerLookingDirection; }
    }

    float jumpVectorSpeed;
    float jumpingPressTime;

    //settings
    [SerializeField]
    private float deafaultWalkingSpeed = 3f;
    public float DeafaultWalkingSpeed
    {
        get { return deafaultWalkingSpeed; }
    }
    [SerializeField]
    private float walkingSpeed = 3f;
    public float WalkingSpeed
    {
        get { return walkingSpeed; }
        set { walkingSpeed = value; }
    }
    [SerializeField]
    private float jumpingForce = 6f;
    public float JumpingForce
    {
        get { return jumpingForce; }
        set { jumpingForce = value; }
    }
    [SerializeField]
    private float deafaultJumpForce = 6f;
    public float DeafaultJumpForce
    {
        get { return deafaultJumpForce; }
    }
    [SerializeField]
    private float interactingTimeDuration = 0.1f;
    [SerializeField]
    private float attackDamagePoint = 5f;
    //rotation
    private float turningAngleValuePerSeconc = 90f;
    private float actualTurningAngel;
    private float forwardWalkingSpeed;
    //delegates & events
    public delegate void LookingDirectionChangeHandler(LookingDirectionName fd);
    public static event LookingDirectionChangeHandler OnLookingChange;
    public delegate void InteractiveBoxEventHandler(float dur, float sec, ActionType act);
    public static event InteractiveBoxEventHandler OnInteraction;
    private float rotationSpeed = 5f;
    protected override void Start()
    {
        
        base.Start();
        if (OnLookingChange != null)
        {
            OnLookingChange(playerLookingDirection);
        }
        actualTurningAngel = 0f;
        _keysInfo.text = "PLAYER WALKING:\n" + "W - forward\n" + "S - backward\n" + "A - right\n" + "D - left\n" + "SPACE - jump\n" + "Z - car on airplane in\n" + "X - attack\n" +"F - fire ball when looking forward";
    }

    public override void ReadInput(InputDataStruct data)
    {
        previosWalkingVector = walkingVector;
        ResetMovingToZero();
        if(data.buttonsTab[3] == true)
        {
            walkingSpeed = 5;
        }
        else
        {
            walkingSpeed = deafaultWalkingSpeed;
        }
        //turning player
        if (data.axesTab[2] != 0)
        {
            actualTurningAngel = turningAngleValuePerSeconc * Time.deltaTime * (data.axesTab[2] > 0 ? 1 : -1);   
        }
        //set vertical movement
        if (data.axesTab[0] != 0f)
        {
            walkingVector += Vector3.forward * data.axesTab[0] * walkingSpeed;
            if (data.axesTab[0] > 0)
            {
                infoText.text = "WALKING - FORWARD";
            }
            else if(data.axesTab[0] < 0)
            {
                infoText.text = "WALKING - BACK";
            }
            
        }

        //set horizontal movement
        if (data.axesTab[1] != 0f)
        {
            walkingVector += Vector3.right * data.axesTab[1] * walkingSpeed;

            if (data.axesTab[1] > 0)
            {
                infoText.text = "WALKING - RIGHT";
            }
            else if (data.axesTab[1] < 0)
            {
                infoText.text = "WALKING - LEFT";
            }
        }

        //set vertical jump
        if (data.buttonsTab[0] == true)
        {
            if (jumpingPressTime == 0f)
            {
                if (CheckIsPlayerOnGround())
                {
                    jumpVectorSpeed = jumpingForce;
                    infoText.text = "JUMP !!!";
                }
            }
            jumpingPressTime += Time.deltaTime;
        }
        else
        {
            jumpingPressTime = 0f;
        }

        //check if interact button is pressed
        if (data.buttonsTab[1] == true)
        {
            if (OnInteraction != null)
            {
                OnInteraction(interactingTimeDuration, 0, ActionType.Interact);
            }
        }

        //check if attack button is pressed
        if (data.buttonsTab[2] == true)
        {
            if (OnInteraction != null)
            {
                OnInteraction(interactingTimeDuration, attackDamagePoint, ActionType.Attack);
            }
        }

        newInput = true;

    }

    void LateUpdate()
    {
        //playerRotation
        _rigidBody.rotation = Quaternion.Euler(_rigidBody.rotation.eulerAngles + new Vector3(0, actualTurningAngel, 0));


        if (!newInput)
        {
            previosWalkingVector = walkingVector;
            ResetMovingToZero();
            jumpingPressTime = 0f;
            infoText.text = "STANDING STILL";
        }
        if (previosWalkingVector != walkingVector)
        { 
            //check if there is a face change
            CheckForLookingDirectionChange();
        }
        _rigidBody.velocity = new Vector3(walkingVector.x, _rigidBody.velocity.y + jumpVectorSpeed, walkingVector.z);
        newInput = false;
        actualTurningAngel = 0f;//reset for next frame
    }

    void ResetMovingToZero()
    {
        walkingVector = Vector3.zero;
        jumpVectorSpeed = 0f;
    }

    //check collider under player
    bool CheckIsPlayerOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + 0.1f);
    }

    void CheckForLookingDirectionChange()
    {
        if (walkingVector == Vector3.zero)
        {
            return;
        }
        if (walkingVector.x == 0 || walkingVector.z == 0)
        {
            //change our facing based on walkVelocity
            ChangeLookingDirection(walkingVector);
        }
        else
        {
            if (previosWalkingVector.x == 0)
            {
                ChangeLookingDirection(new Vector3(walkingVector.x, 0, 0));
            }
            else if (previosWalkingVector.z == 0)
            {
                ChangeLookingDirection(new Vector3(0, 0, walkingVector.z));
            }
            else
            {
                Debug.LogWarning("Unexpected walkVelocity value.");
                ChangeLookingDirection(walkingVector);
            }
        }
    }

    void ChangeLookingDirection(Vector3 _direction)
    {
        if (_direction.z != 0)
        {
            playerLookingDirection = (_direction.z > 0) ? LookingDirectionName.FORWARD : LookingDirectionName.BACKWARD;
        }
        else if (_direction.x != 0)
        {
            playerLookingDirection = (_direction.x > 0) ? LookingDirectionName.RIGHT : LookingDirectionName.LEFT;
        }

        //call change facing event
        if (OnLookingChange != null)
        {
            OnLookingChange(playerLookingDirection);
        }
        Debug.Log(playerLookingDirection);
    }
}
