using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_playerRigidbody2D;
    private Transform m_playerSword;
    private float m_playerSpeed = 500.0f;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerRigidbody2D = GetComponent<Rigidbody2D>();
        m_playerSword = GetComponentInChildren<Transform>().Find("PlayerSword");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator.SetBool("CanWalk", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            m_animator.SetFloat("Speed", 0.0f);
            m_animator.SetBool("IsAttacking", true);
            return;
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

    private void FixedUpdate()
    {
        // Keeps the animation from playing if the player is moving in two oposite directions
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            return;
        }

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += new Vector2(0.0f, 1.0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += new Vector2(0.0f, -1.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += new Vector2(-1.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += new Vector2(1.0f, 0.0f);
        }

        m_playerRigidbody2D.velocity = m_playerSpeed * Time.fixedDeltaTime * direction.normalized;
    }

    public void OnAttackEnd()
    {
        m_animator.SetBool("IsAttacking", false);
    }
}
