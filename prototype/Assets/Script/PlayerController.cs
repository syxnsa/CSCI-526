using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public ColorType playerColor;  // The current color of the player
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float deathHeight = -5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        UpdateColor();  // Initialize player color

        GameObject deathZone = GameObject.FindGameObjectWithTag("DeathZone");
        if (deathZone != null)
        {
            deathHeight = deathZone.transform.position.y; 
        }
        else
        {
            Debug.LogError("DeathZone not found in scene! Please add an object with tag 'DeathZone'.");
            deathHeight = -5f; 
        }
    }

    void Update()
    {
        // Handle player movement
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    void FixedUpdate()
    {
        UpdateCollisionState();
        CheckDeath();

    }

    private void CheckDeath()
    {
        if (transform.position.y < deathHeight)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Change the player's color
    public void ChangeColor(ColorType newColor)
    {
        playerColor = newColor;
        UpdateColor();
    }

    private void UpdateColor()
    {
        sr.color = ColorManager.GetColor(playerColor);
    }

    // Raycast method to check if the player is on a valid platform
    private bool IsGrounded()
    {
        float rayLength = 0.2f;  // Adjust ray length for accuracy
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f); // Cast from slightly below player
        Vector2 direction = Vector2.down;

        int platformLayer = LayerMask.GetMask("Platform");  // Make sure Platform layer is assigned in Unity

        // Debugging: Draw the ray in Scene view
        Debug.DrawRay(origin, direction * rayLength, Color.green);

        // Perform the raycast, ignoring other layers
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayLength, platformLayer);

        if (hit.collider != null)
        {


            if (hit.collider.CompareTag("Platform")) // Check if it's a platform
            {
                Platform platform = hit.collider.GetComponent<Platform>();
                if (platform != null)
                {
                    return platform.platformColor == playerColor; // Return true only if color matches
                }
            }
        }

        return false; // No valid platform detected
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();

            if (platform != null)
            {
                if (platform.platformColor == playerColor)
                {
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false); // Enable collision
                }
                else
                {
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true); // Disable collision
                }
            }
        }
    }

    // Handle collision exit
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();

            if (platform != null)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false); // Reset collision
            }
        }
    }

    private void UpdateCollisionState()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false; // Ignore triggers
        filter.SetLayerMask(LayerMask.GetMask("Platform")); // Only detect platforms
        filter.useLayerMask = true;

        Collider2D[] colliders = new Collider2D[5]; // Store detected collisions
        int collisionCount = GetComponent<Collider2D>().OverlapCollider(filter, colliders);

        for (int i = 0; i < collisionCount; i++)
        {
            Collider2D col = colliders[i];

            if (col.CompareTag("Platform")) // Ensure it's a platform
            {
                Platform platform = col.GetComponent<Platform>();

                if (platform != null)
                {
                    if (platform.platformColor == playerColor)
                    {
                        Physics2D.IgnoreCollision(col, GetComponent<Collider2D>(), false); // Enable collision
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(col, GetComponent<Collider2D>(), true); // Disable collision
                        Debug.Log("Ignoring collision with: " + platform.gameObject.name);
                    }
                }
            }
        }
    }

}
