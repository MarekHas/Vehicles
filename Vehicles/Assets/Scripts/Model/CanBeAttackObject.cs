using UnityEngine;
using System.Collections;

public class CanBeAttackObject : MonoBehaviour, ICanBeAttacked
{
    [SerializeField] private float healthPoints = 50f;
    [SerializeField] private Gradient healthColor;
    private Material material;
    private float actualHealthPoints;

    // Use this for initialization
    void Start()
    {
        gameObject.layer = 10;//Attack target layer
        material = GetComponent<MeshRenderer>().material;
        actualHealthPoints = healthPoints;
        HealthColorChanging();
    }

    void HealthColorChanging()
    {
        material.color = healthColor.Evaluate(actualHealthPoints / healthPoints);
    }

    public void ReciveDamage(float damage)
    {
        actualHealthPoints -= damage;
        actualHealthPoints = Mathf.Max(actualHealthPoints, 0);
        HealthColorChanging();
    }
}
