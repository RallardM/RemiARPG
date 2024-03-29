using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TrailRenderer m_trailRenderer;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_playerRigidbody2D;
    private Transform m_playerSword;
    private Transform m_playerHitBox;

    private Vector3 m_invertVector3X = new(-0.1f, 0.1f, 0.1f);
    private Vector3 m_revertVector3X = new(0.1f, 0.1f, 0.1f);
    private Vector2 m_movement = Vector2.zero;
    private Vector2 m_moveUp = new(0.0f, 1.0f);
    private Vector2 m_moveDown = new(0.0f, -1.0f);
    private Vector2 m_moveLeft = new(-1.0f, 0.0f);
    private Vector2 m_moveRight = new(1.0f, 0.0f);

    private float m_playerSpeed = 500.0f;
    private float m_dashingPower = 6.0f;
    private float m_dashingTime = 0.4f;
    private float m_dashingCooldown = 1.0f;
    private float m_maxSpeed = 1.0f;
    private float m_minSpeed = 0.0f;

    private bool m_canDash = true;
    private bool m_isDashing = false;

    private void Awake()
    {
        m_playerRigidbody2D = GetComponent<Rigidbody2D>();
        Transform playerSprite = GetComponentInChildren<Transform>().Find("Sprite");
        m_spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        m_animator = playerSprite.GetComponent<Animator>();
        m_playerSword = playerSprite.GetComponentInChildren<Transform>().Find("PlayerSword");
        m_trailRenderer= GetComponentInChildren<TrailRenderer>();
        m_playerHitBox = transform.GetComponentInChildren<Transform>().Find("PlayerHitBox");
    }

    // Update is called once per frame
    void Update()
    {
        // Keeps the animation from playing if the player is dashing
        if(m_isDashing)
        {
            return;
        }

        // Keeps the animation from playing if the player is dead
        if (m_animator.GetBool("IsDying"))
        {
            m_animator.SetFloat("Speed", m_minSpeed);
            return;
        }

        AnimPlayer();
    }

    private void FixedUpdate()
    {
        // Keeps the player from moving if the player is dashing
        if (m_isDashing)
        {
            return;
        }

        // Keeps the player from moving if the player is dead
        if (m_animator.GetBool("IsDying"))
        {
            m_playerRigidbody2D.velocity = Vector2.zero;
            return;
        }
        
        MovePlayer();
    }

    private void AnimPlayer()
    {
        UpdateAttacking();

        UpdateDashing();

        UpdateWalking();
    }

    private void UpdateWalking()
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_animator.SetFloat("Speed", m_maxSpeed);
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_animator.SetFloat("Speed", m_maxSpeed);
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_spriteRenderer.flipX = true;
            // Source : https://www.youtube.com/watch?v=BvIC5KMjBVE
            // Flips the sword and the second castshadow in hitbox when the player is facing left
            m_playerSword.localScale = new Vector3(transform.localScale.x * m_invertVector3X.x, transform.localScale.y * m_invertVector3X.y, transform.localScale.z * m_invertVector3X.z);
            m_playerHitBox.localScale = new Vector3(transform.localScale.x * m_invertVector3X.x, transform.localScale.y * m_invertVector3X.y, transform.localScale.z * m_invertVector3X.z);
            m_animator.SetFloat("Speed", m_maxSpeed);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_spriteRenderer.flipX = false;
            // Flips the sword and the second castshadow in hitbox when the player is facing left
            m_playerSword.localScale = new Vector3(transform.localScale.x * m_revertVector3X.x, transform.localScale.y * m_revertVector3X.y, transform.localScale.z * m_revertVector3X.z);
            m_playerHitBox.localScale = new Vector3(transform.localScale.x * m_revertVector3X.x, transform.localScale.y * m_revertVector3X.y, transform.localScale.z * m_revertVector3X.z);
            m_animator.SetFloat("Speed", m_maxSpeed);
            return;
        }

        m_animator.SetFloat("Speed", m_minSpeed);
    }

    private void UpdateDashing()
    {
        // Space or LeftShift is pressed and the player can dash, plays the dash animation.
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift)) && m_canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void UpdateAttacking()
    {
        // If the player is clicking left and the character was not attacking, plays the attack animation.
        if (Input.GetKey(KeyCode.Mouse0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            m_animator.SetFloat("Speed", m_minSpeed);
            m_animator.SetBool("IsAttacking", true);
            return;
        }
    }

    private void MovePlayer()
    {
        // Keeps the animation from playing if the player is moving in two oposite directions
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            return;
        }

        m_movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            m_movement += m_moveUp;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_movement += m_moveDown;
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_movement += m_moveLeft;
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_movement += m_moveRight;
        }

        m_playerRigidbody2D.velocity = m_playerSpeed * Time.fixedDeltaTime * m_movement.normalized;
    }

    // Source : https://www.youtube.com/watch?v=0v_H3oOR0aU
    private IEnumerator Dash()
    {
        m_canDash = false;
        m_isDashing = true;
        m_playerRigidbody2D.velocity = new Vector2(transform.localScale.x * m_movement.x, transform.localScale.y * m_movement.y) * m_dashingPower;
        m_trailRenderer.emitting = true;
        yield return new WaitForSeconds(m_dashingTime);
        m_trailRenderer.emitting = false;
        m_isDashing = false;
        yield return new WaitForSeconds(m_dashingCooldown);
        m_canDash = true;
    }
}
