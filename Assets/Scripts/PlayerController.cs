using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{


    private PlayerInput playerInput;
    private Vector2 movementVector;
    private Rigidbody2D rb;
    public float movementSpeedValue;
    public float thrustForce = 2.0f;
    public float maxSpeed = 5f;

    public GameObject BoostSprite;
    public ParticleSystem BoostSpriteParticle;

    private float elapsedTime = 0f;
    public float scoreMultiplier = 2f;
    int score;

    public GameObject explosionEffect;

    Label scoreText;
    Label highScoreText;

    public UIDocument uIDocument;

    private Button restartButton;

    public GameObject spatulaLeft;
    public GameObject spatulaRight;

    void Reset()
    {
        movementSpeedValue = 200;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        BoostSpriteParticle = BoostSprite.GetComponent<ParticleSystem>();


        // Init Score text type
        scoreText = uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uIDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        restartButton = uIDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        highScoreText.style.display = DisplayStyle.None;

        restartButton.clicked += ReloadScene;
    }

    // Update is called once per frame
    void Update()
    {
        updateMovement();
        updateScore();
    }

    // Use for update player movement force by input.
    void updateMovement()
    {

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;

            Debug.Log("Left Button Pressed!");


            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

            }
            else
            {
                rb.AddForce(direction * thrustForce * Time.deltaTime);

            }

            BoostSpriteParticle.Play();

            // animate rotation
            int rotateValue = 15;

            float rotatePingPong = Mathf.PingPong(Time.time, rotateValue);

            spatulaLeft.transform.Rotate(0, 0, rotatePingPong);
            spatulaRight.transform.Rotate(0, 0, rotatePingPong);


        }
        else
        {
            BoostSpriteParticle.Stop();


        }
    }
    // Use for update score and ui
    void updateScore()
    {
        if (gameObject.activeSelf)
        {
            elapsedTime += Time.deltaTime;
            score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
            scoreText.text = "Score  " + score;

            Debug.Log(score);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Instantiate(explosionEffect, transform.position, transform.rotation);
            restartButton.style.display = DisplayStyle.Flex;
            highScoreText.style.display = DisplayStyle.Flex;

            Time.timeScale = 0;
            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            highScoreText.text = "High Score  " + score;


        }
    }

    // Update force to object
    void FixedUpdate()
    {
        // Debug.Log("Rigid Body Add force : " + movementVector);
    }

    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
    }


    void OnGUI()
    {
        GUILayout.Label("Current Input Direction : " + movementVector);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
