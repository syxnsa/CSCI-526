using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;  
    public float lifetime = 2f; 

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public ColorType bulletColor = ColorType.Yellow;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        UpdateBulletColor();

        rb.velocity = transform.right * speed;

        Destroy(gameObject, lifetime);
    }

    private void UpdateBulletColor()
    {
        if (sr != null)
        {
            sr.color = ColorManager.GetColor(bulletColor);
        }
    }

    public void SetBulletColor(ColorType newColor)
    {
        bulletColor = newColor;
        UpdateBulletColor();
    }

    // Handle collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform")) 
        {
            Platform platform = collision.GetComponent<Platform>();
            if (platform != null)
            {
                platform.ChangeColor(bulletColor); 
            }

            Destroy(gameObject); 
        }
    }
}
