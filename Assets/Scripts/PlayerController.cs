using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float thrustForce = 1f;
    public float maxSpeed = 10f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Left click detection
        // if (Mouse.current.leftButton.wasPressedThisFrame)
        // Difference between wasPressedThisFrame and isPressed:
        // wasPressedThisFrame is true only for the frame when the button was pressed,
        // while isPressed is true for every frame the button is held down.
        if (Mouse.current.leftButton.isPressed)
        {
            // World space position of the click
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            // Player faces mouse position (in 2D)
            // Normalize the direction vector to get constant speed
            Vector2 direction = (mousePosition - transform.position).normalized;
            transform.up = direction;

            // Apply thrust force in the direction of the mouse click
            rb.AddForce(direction * thrustForce, ForceMode2D.Impulse);

            // Limit player's maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }

    // Collision detection with obstacles and borders
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
