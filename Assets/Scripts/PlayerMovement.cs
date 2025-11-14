using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;
    public float inputX, inputY, speed, lastX, lastY;
    private Animator anim;
    private SpriteRenderer sr;

    public float kbForce = 5f;
    public float kbTime = 0.2f;
    private bool isKnockedBack = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Animate();
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            rb.velocity = new Vector2(inputX * speed, inputY * speed);
        }
    }

    void GetInput()
    {
        if (isKnockedBack)
        {
            inputX = 0;
            inputY = 0;
            return;
        }

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0)
        {
            lastX = inputX;
        }

        if (inputY != 0)
        {
            lastY = inputY;
        }
    }

    void Animate()
    {
        anim.SetFloat("InputX", inputX);
        anim.SetFloat("InputY", inputY);
        anim.SetFloat("LastX", lastX);
        anim.SetFloat("LastY", lastY);
        anim.SetFloat("Magnitude", rb.velocity.magnitude);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Skeleton"))
        {
            HandleMonsterContact(collision);
        }
        else if (collision.gameObject.CompareTag("Goblin") || collision.gameObject.CompareTag("Cow") || collision.gameObject.CompareTag("Sheep") || collision.gameObject.CompareTag("Duck") || collision.gameObject.CompareTag("Bird"))
        {
            HandleSoundContact(collision.gameObject);
        }
    }

    void HandleMonsterContact(Collision2D collision)
    {
        Debug.Log("Player is hurt.");

        Vector2 direction = (transform.position - collision.transform.position).normalized;

        if (direction.x > 0)
        {
            anim.SetFloat("HurtDirection", -1f);
        }
        else if (direction.x < 0)
        {
            anim.SetFloat("HurtDirection", 1f);
        }

        anim.SetTrigger("IsHurt");

        StartCoroutine(Knockback(direction));
    }

    IEnumerator Knockback(Vector2 direction)
    {
        isKnockedBack = true;

        rb.AddForce(direction * kbForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(kbTime);

        rb.velocity = Vector2.zero;

        isKnockedBack = false;
    }

    void HandleSoundContact(GameObject contactObject)
    {
        string tag = contactObject.tag;
        string sound = "";

        if (tag == "Goblin")
        {
            sound = "GRRRRR (Goblin noises)";
        }
        else if (tag == "Cow")
        {
            sound = "MOO (Cow noises)";
        }
        else if (tag == "Sheep")
        {
            sound = "BAA (Sheep noises)";
        }
        else if (tag == "Duck")
        {
            sound = "QUACK (Duck noises)";
        }
        else if (tag == "Bird")
        {
            sound = "CHIRP (Bird noises)";
        }

        Debug.Log(sound);
    }
}