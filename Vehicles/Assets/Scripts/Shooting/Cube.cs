using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField]
    private int fullHealth = 1;
    private int actualHealth;
    [SerializeField]
    private int pointForDestoryCube = 1;
    bool isCubeDestroyed;
    float createTime = 1f;

    public event System.Action CubeDestroyedEvent;

    [SerializeField]
    private float minSpeed = 1;
    [SerializeField]
    private float maxSpeed = 10;

    public float choosenSpeed;

    //change Cube Scale
    private float _currentScale = InitScale;
    private const float TargetScaleRED = 1f;
    private const float TargetScaleBLUE = 2f;
    private const float InitScale = 0f;
    private const int FramesCount = 50;
    private const float AnimationTimeSeconds = 1f;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dxRED = (TargetScaleRED - InitScale) / FramesCount;
    private float _dxBLUE = (TargetScaleRED - InitScale) / FramesCount;
    private bool _upScale = true;

    [SerializeField]
    private bool isRed = false;

    void Start()
    {
        actualHealth = fullHealth;
        choosenSpeed = Random.Range(minSpeed, maxSpeed);
        StartCoroutine(ChangeScale());
    }

    private IEnumerator ChangeScale()
    {
        if (isRed)
        {
            while (_upScale)
            {
                _currentScale += _dxRED;
                if (_currentScale > TargetScaleRED)
                {
                    _upScale = false;
                    _currentScale = TargetScaleRED;
                }
                transform.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }
        }
        else
        {
            while (_upScale)
            {
                _currentScale += _dxBLUE;
                if (_currentScale > TargetScaleBLUE)
                {
                    _upScale = false;
                    _currentScale = TargetScaleBLUE;
                }
                transform.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }
        }

    }


    public void CubeTakeDamge(int damgePoints)
    {
        actualHealth -= damgePoints;
        if (actualHealth <= 0)
        {
            CubeDestroyed();
        }
    }

    protected void CubeDestroyed()
    {
        isCubeDestroyed = true;

        if (CubeDestroyedEvent != null)
        {
            CubeDestroyedEvent();
        }
        gameObject.SetActive(false);

        //PointsCounter.scorePoints += pointForDestoryCube;
    }
}
