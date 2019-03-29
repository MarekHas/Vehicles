using UnityEngine;

public class BallShooting : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float ballImpulse = 10f;
    [SerializeField] private GameObject _ballSpawnPoint;
    private WalkingController _walkingController;

    private void Start()
    {
        _walkingController = GetComponent<WalkingController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _walkingController.PlayerLookingDirection == LookingDirectionName.FORWARD)
        {

            GameObject ballThrown = (GameObject)Instantiate(_ballPrefab, _ballSpawnPoint.transform.position + _ballSpawnPoint.transform.forward, _ballSpawnPoint.transform.rotation);
            ballThrown.GetComponent<Rigidbody>().AddForce(_ballSpawnPoint.transform.forward * ballImpulse, ForceMode.Impulse);
        }
    }
}