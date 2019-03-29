using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour, IInteractive
{
    private Material _material;
    // Use this for initialization
    void Start()
    {
        gameObject.layer = 10;//Interactive layer
        _material = GetComponent<MeshRenderer>().material;
    }

    public void OnInteract()
    {
        //when interact change color
        _material.color = Color.green;
    }

}
