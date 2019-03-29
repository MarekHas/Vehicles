using UnityEngine;
using UnityEngine.UI;
public class Pickup : MonoBehaviour{
    [SerializeField] private float speedModifierValue=0.5f;
    [SerializeField] private float jumpModifierValue=0.5f;
    [SerializeField] private Text _infoText;
    public void Update()
    {
        transform.Rotate(0f,Time.deltaTime *25f, 0f);
    }
    public void OnTriggerEnter(Collider _collider)
    {
        if (_collider.tag == "Player")
        {
            //Debug.Log("BEFORE speed  : " + _collider.GetComponent<WalkingController>().walkingSpeed);
            //Debug.Log("Palyer");
            var walkSpeed = _collider.GetComponent<WalkingController>().WalkingSpeed;
            var jumpForce = _collider.GetComponent<WalkingController>().JumpingForce;
            //var defaultSpeed = _collider.GetComponent<WalkingController>().deafaultWalkSpeed;
            //walkSpeed =10;
            //jumpForce = jumpForce + jumpForce / 2;
            _collider.GetComponent<WalkingController>().WalkingSpeed += 3f;
            _collider.GetComponent<WalkingController>().JumpingForce += jumpForce * jumpModifierValue;
            //walkSpeed += walkSpeed * speedModifierValue;
            //jumpForce += jumpForce * jumpModifierValue;
            _infoText.text = "PLAYER POWER UP !!!\n" + "Walk speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed + "\n" + "JUMP speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed;
            Debug.Log("Walk speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed);
            Debug.Log("JUMP speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed);
        }
    }
}
