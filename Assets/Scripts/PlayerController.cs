using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// TODO Change the entire game concept

public class Player : MonoBehaviour
{
    public InputAction moveForward; // For mobile
    public InputAction lookPosition; // For mobile
    public float thrustForce = 1f;
    public float maxSpeed = 10f;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    public GameObject explossionEffect;
    public GameObject backgroundSatellites;
    public GameObject borderParent;
    private Label scoreLabel;
    private Label highScoreLabel;
    private GameObject shipFlair;

    private Button restartButton;
    private float elapsedTime = 0f;
    private float score = 0f;
    private float scoreMultiplier = 5f;

    // Start is called before the first frame update
    void Start()
    {
        moveForward.Enable();
        lookPosition.Enable();
        Instantiate(backgroundSatellites);
        rb = GetComponent<Rigidbody2D>();
        shipFlair = transform.Find("Flair")?.gameObject; // ? operator checks if the child exists before trying to access it
        if (uiDocument != null)
        {
            scoreLabel = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
            highScoreLabel = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton"); // Q is a shorthand for Query, which finds the first element
                                                                                     // of the specified type and name in the UI hierarchy.

            // Initialize high score label with the saved value so it's correct after restarts
            if (highScoreLabel != null)
            {
                int savedHigh = PlayerPrefs.GetInt("HighScore", 0); // 0 here is the default value if no high score is saved yet
                highScoreLabel.text = $"HIGH SCORE: {savedHigh}";
                highScoreLabel.style.display = DisplayStyle.None; // Hide the high score label initially
            }

            if (restartButton != null)
            {
                restartButton.style.display = DisplayStyle.None; // Hide the restart button initially
                restartButton.clicked += RestartScene;
            }
        }
    }

    // FixedUpdate is called once per physics update, it is better for consistent rigidbody movement 
    void FixedUpdate()
    {
        MovePlayer();
        CalculateScore();
        SaveHighScore();
    }

    void MovePlayer()
    {
        // Left click detection
        // if (Mouse.current.leftButton.wasPressedThisFrame)
        // Difference between wasPressedThisFrame and isPressed:
        // wasPressedThisFrame is true only for the frame when the button was pressed,
        // while isPressed is true for every frame the button is held down.
        // if (Mouse.current.leftButton.isPressed)
        if (moveForward.IsPressed())
        {
            // World space position of the click
            // Vector3 mousePosition = Camera.main.screentoworldpoint(mouse.current.position.value);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());

            // Player faces mouse position (in 2D)
            // Normalize the direction vector to get constant speed
            Vector2 direction = (mousePosition - transform.position).normalized;
            transform.up = direction;

            // Apply thrust force in the direction of the mouse click
            rb.AddForce(direction * thrustForce, ForceMode2D.Impulse);
            if (shipFlair != null)
            {
                shipFlair.SetActive(!shipFlair.activeSelf);
            }

            // Limit player's maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }

    void CalculateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        if (scoreLabel != null)
            scoreLabel.text = $"SCORE: {score}";
    }

    void SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", (int)score);
            if (highScoreLabel != null)
                highScoreLabel.text = $"HIGH SCORE: {score}";
        }
    }

    void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Collision detection with obstacles and borders
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"Score this run: {score}");
        //Debug.Log($"High score: {PlayerPrefs.GetInt("HighScore")}");
        // Ensure the UI shows the stored high score when the player dies
        int savedHigh = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreLabel != null)
        {
            highScoreLabel.text = $"HIGH SCORE: {savedHigh}";
            highScoreLabel.style.display = DisplayStyle.Flex; // Show the high score label
        }
        if (restartButton != null)
            restartButton.style.display = DisplayStyle.Flex;

        Instantiate(explossionEffect, transform.position, Quaternion.identity); // Quaternion.identity means no rotation (2D)
        Destroy(gameObject);
        borderParent.SetActive(false); 
    }
}
