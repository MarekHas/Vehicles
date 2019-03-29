using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SlowDown: MonoBehaviour{
    [SerializeField] private float speedModifierValue=0.5f;
    [SerializeField] private float jumpModifierValue=0.5f;
    [SerializeField] private Text _infoText;
    public void Update()
    {
        transform.Rotate(0f,Time.deltaTime *25f, 0f);
    }
    public void OnTriggerEnter(Collider _collider)
    {
        if(_collider.tag == "Player")
        {
        
            _collider.GetComponent<WalkingController>().WalkingSpeed = _collider.GetComponent<WalkingController>().DeafaultWalkingSpeed;
            _collider.GetComponent<WalkingController>().JumpingForce = _collider.GetComponent<WalkingController>().DeafaultJumpForce;
            //walkSpeed += walkSpeed * speedModifierValue;
            //jumpForce += jumpForce * jumpModifierValue;
            StartCoroutine(TextDIsplay(_collider));
            //_infoText.text = "PLAYER POWER DOWN !!!\n" + "Walk speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed + "\n" + "JUMP speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed;
            Debug.Log("Walk speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed);
            Debug.Log("JUMP speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed);
        }
    }
    IEnumerator TextDIsplay (Collider _collider)
    {
        _infoText.text = "PLAYER POWER DOWN !!!\n" + "Walk speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed + "\n" + "JUMP speed  : " + _collider.GetComponent<WalkingController>().WalkingSpeed;
        yield return new WaitForSeconds(.5f);
    }
}
