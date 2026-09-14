using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float minSize = 0.5f;
    private float maxSize = 2.0f;
    Rigidbody2D rb;
    private float minSpeed = 150f;
    private float maxSpeed = 400f;
    private float minTorque = -12f;
    private float maxTorque = 12f;
    public GameObject collisionEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        float randomSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;
        float randomTorque = Random.Range(minTorque, maxTorque);
        transform.localScale = new Vector3(randomSize, randomSize, 44);

        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle;
        rb.AddForce(randomDirection * randomSpeed);
        rb.AddTorque(randomTorque);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject collisionEffect = Instantiate(collisionEffectPrefab, contactPoint, Quaternion.identity);

        // Destroy the effect after 1 second
        Destroy(collisionEffect, 0.5f);
    }
}
