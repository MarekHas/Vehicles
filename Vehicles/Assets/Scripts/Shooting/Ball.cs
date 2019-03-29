using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{

    private float ballLifeTime = 10f;
    private float changeColor = 0.0023f;
    private int ballDamagePoints = 1;

    void Update()
    {
        for (int i = 0; i < ballLifeTime; i++)
        {
            ballLifeTime -= Time.deltaTime;
            //this.GetComponent<Renderer>().material.color -= new Color(changeColor, changeColor, changeColor);
        }

        if (ballLifeTime <= 0)
        {
            BallDestroy();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Cube hittedTarget = collision.gameObject.GetComponent<Cube>();

            hittedTarget.rb.AddForce(new Vector3(2,1,3),ForceMode.Impulse);
            //hittedTarget.CubeTakeDamge(ballDamagePoints);
            BallDestroy();
            Debug.Log("hitted");
        }
    }

    public void BallDestroy()
    {
        Destroy(gameObject);
    }
}

