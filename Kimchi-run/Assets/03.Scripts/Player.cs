using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JumpForce;

    [Header("References")]
    public Rigidbody2D PlayerRigidBody;

    public Animator PlayerAnimator;

    public BoxCollider2D PlayerCollider;

    private bool isGrounded = true;

    public int lives = 3;

    private bool isInvincible = false;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("state", 1);
        }
    }

    void KillPlayer()
    {
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;
        PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
    }

    void Hit()
    {
        lives -= 1;
        if(lives == 0)
        {
            KillPlayer();
        }
    }

    void Heal()
    {
        lives = Mathf.Min(3, lives + 1);
    }

    void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    void StopInvincible()
    {
        isInvincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Platform")
        {
            if (!isGrounded)
            {
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded =true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "enemy")
        {
            if(!isInvincible)
            {
                Destroy(collider.gameObject);
                Hit();
            }
        }
        else if(collider.gameObject.tag == "food")
        {
            Destroy(collider.gameObject);
            Heal();
        }
        else if(collider.gameObject.tag == "golden")
        {
            Destroy(collider.gameObject);
            StartInvincible();
        }
    }
}


