using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TrailRenderer m_trailRenderer;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_playerRigidbody2D;
    private Transform m_playerSword;

    private Vector2 m_movement = Vector2.zero;

    private float m_playerSpeed = 500.0f;
    private float m_dashingPower = 6.0f;
    private float m_dashingTime = 0.4f;
    private float m_dashingCooldown = 1.0f;

    private bool m_canDash = true;
    private bool m_isDashing = false;
   

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerRigidbody2D = GetComponent<Rigidbody2D>();
        m_playerSword = GetComponentInChildren<Transform>().Find("PlayerSword");
        m_trailRenderer= GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isDashing)
        {
            return;
        }

        if (m_animator.GetBool("IsDying"))
        {
            m_animator.SetFloat("Speed", 0.0f);
            return;
        }

        AnimPlayer();
    }

    private void FixedUpdate()
    {
        if (m_isDashing)
        {
            return;
        }

        if (m_animator.GetBool("IsDying"))
        {
            m_playerRigidbody2D.velocity = Vector2.zero;
            return;
        }
        
        MovePlayer();
    }

    private void AnimPlayer()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            m_animator.SetFloat("Speed", 0.0f);
            m_animator.SetBool("IsAttacking", true);
            return;
        }

        if (Input.GetKey(KeyCode.Space) && m_canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKey(KeyCode.W))
        {
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_spriteRenderer.flipX = true;
            // Source : https://www.youtube.com/watch?v=BvIC5KMjBVE
            m_playerSword.localScale = new Vector3(transform.localScale.x * -0.1f, transform.localScale.y * 0.1f, transform.localScale.z * 0.1f);
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_spriteRenderer.flipX = false;
            m_playerSword.localScale = new Vector3(transform.localScale.x * 0.1f, transform.localScale.y * 0.1f, transform.localScale.z * 0.1f);
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }

        m_animator.SetFloat("Speed", 0.0f);
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
            m_movement += new Vector2(0.0f, 1.0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_movement += new Vector2(0.0f, -1.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            m_movement += new Vector2(-1.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_movement += new Vector2(1.0f, 0.0f);
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

    public void OnAttackEnd()
    {
        m_animator.SetBool("IsAttacking", false);
    }

    public void OnEndOfDamageAnim()
    {
        m_animator.SetBool("IsReceivingDamage", false);
    }
}
