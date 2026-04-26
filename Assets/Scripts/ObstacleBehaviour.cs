using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;


public class Obstacles : MonoBehaviour
{
    public float minSizeRan;
    public float maxSizeRan;

    public float minSpeed;
    public float maxSpeed;

    public float maxSpinSpeed = 10f;

    public GameObject bounceFx;

    Rigidbody2D rb;

    private int maxGrow = 4;
    private int currentGrow = 0;

    public GameObject audioSourceObjectGrow;
    public GameObject audioSourceObjectContact;

    private AudioSource audioSourceGrow;
    private AudioSource audioSourceContact;

    public GameObject spawnObstacle;
    private SpriteRenderer spriteRenderer;
    private Color[] colorArray = new Color[] {
        new Color(1f, .9f, .9f),
        new Color(1f, .8f, .8f),
        new Color(1f, .7f, .7f),
        new Color(1f, .1f, .1f),
};

    private Collider2D collider;

    void Reset()
    {
        minSizeRan = .7f;
        maxSizeRan = 2.0f;
        minSpeed = 80f;
        maxSpeed = 100f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSourceGrow = audioSourceObjectGrow.GetComponent<AudioSource>();
        audioSourceContact = audioSourceObjectContact.GetComponent<AudioSource>();



        // Random Size
        float randSizeValue = Random.Range(minSizeRan, maxSizeRan);
        transform.localScale = new Vector3(randSizeValue, randSizeValue, 1);

        // Random direction
        Vector2 initDirectionVector = Random.insideUnitCircle;

        // Random Speed
        float randSpeedValue = Random.Range(minSpeed, maxSpeed) / randSizeValue;

        // Random Rotation
        float randRotateValue = Random.Range(-maxSpinSpeed, maxSpinSpeed);

        // Init Force Direction and Rotation
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(initDirectionVector * randSpeedValue);
        rb.AddTorque(randRotateValue);

        // Get Sprite
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get collider 2d
        collider = GetComponent<Collider2D>();
        collider.enabled = true;

    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Play bounce fx
        Vector2 contactPoint = collision.GetContact(0).point;
        GameObject bounceFxInst = Instantiate(bounceFx, contactPoint, Quaternion.identity);

        Destroy(bounceFxInst, 1f);

        // Grow 1
        if (currentGrow < maxGrow)
        {
            audioSourceContact.Play();

            Debug.Log("Play Sound Contact");
            Debug.Log("AudioSource = " + audioSourceContact);
            Debug.Log("Clip = " + audioSourceContact.clip);
            Debug.Log("Volume = " + audioSourceContact.volume);
            currentGrow += 1;
            // Color color = spriteRenderer.color;
            // color.r = 0.1f;
            spriteRenderer.color = colorArray[currentGrow - 1];
        }
        else
        {
            audioSourceGrow.Play();

            collider.enabled = false;

            Instantiate(spawnObstacle, transform.position, Quaternion.identity);
            Instantiate(spawnObstacle, transform.position, Quaternion.identity);

            // grow new
            gameObject.SetActive(false);
            spriteRenderer.color = colorArray[0];
        }


    }




}
