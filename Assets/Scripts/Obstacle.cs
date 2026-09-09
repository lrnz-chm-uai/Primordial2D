using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    Rigidbody2D rb;
    public float minSpeed = 150f;
    public float maxSpeed = 300f;
    public float minTorque = -10f;
    public float maxTorque = 10f;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
