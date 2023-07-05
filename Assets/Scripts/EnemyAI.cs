// Source : https://youtu.be/2xaQYZW6LLA

using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private LayerMask m_playerLayerMask;
    private Transform m_playerTransform;
    private Transform m_enemyTransform;
    private Transform m_charactersTransfom;
    private Rigidbody2D m_enemyRigidbody2D;
    private Transform m_enemySword;
    private Animator m_animator;
    private Vector2 m_movement;
    private Vector3 m_direction;
    private readonly float m_enemySpeed = 300.0f;
    private readonly float m_checkRadius = 9.0f;
    private readonly float m_attackRadius = 2.0f;
    private bool m_isInChaseRange = false;
    private bool m_isInAttackRange = false;

    private void Awake()
    {
        m_enemyRigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_enemyTransform = transform;
        m_charactersTransfom = m_enemyTransform.transform.parent;
        m_playerTransform = m_charactersTransfom.Find("Player");
        m_playerLayerMask = LayerMask.GetMask("Player");
        m_enemySword = GetComponentInChildren<Transform>().Find("EnemySword");
    }

    private void Update()
    {
        if (m_animator.GetBool("IsDying") == true)
        {
            return;
        }

        AnimEnemy();
    }

    private void FixedUpdate()
    {
        if (m_animator.GetBool("IsDying"))
        {
            m_enemyRigidbody2D.velocity = Vector2.zero;
            return;
        }

        MoveEnemy();
    }

    private void AnimEnemy()
    {
        m_animator.SetBool("IsWalking", m_isInChaseRange);

        m_isInChaseRange = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_playerLayerMask);
        m_isInAttackRange = Physics2D.OverlapCircle(transform.position, m_attackRadius, m_playerLayerMask);

        m_direction = m_playerTransform.position - transform.position;

        float angleToPlayer = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        m_direction.Normalize();
        m_movement = m_direction;

        if (m_direction.x < 0 && !m_animator.GetBool("IsDying"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_enemySword.localScale = new Vector3(transform.localScale.x * -0.1f, transform.localScale.y * 0.1f, transform.localScale.z * 0.1f);
        }
        else if (m_direction.x > 0 && !m_animator.GetBool("IsDying"))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_enemySword.localScale = new Vector3(transform.localScale.x * 0.1f, transform.localScale.y * 0.1f, transform.localScale.z * 0.1f);
        }

        // Source : https://docs.unity3d.com/ScriptReference/Animator.GetCurrentAnimatorStateInfo.html
        if (m_isInAttackRange && !m_animator.GetBool("IsAttacking") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            Debug.Log("IsAttacking");
            m_animator.SetBool("IsWalking", false);
            m_animator.SetBool("IsAttacking", true);
            return;
        }
    }

    private void MoveEnemy()
    {
        if (m_isInChaseRange && !m_isInAttackRange)
        {
            m_enemyRigidbody2D.velocity = m_enemySpeed * Time.fixedDeltaTime * m_movement;
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }
  
        //m_enemyRigidbody2D.velocity = Vector2.zero;
        //m_animator.SetFloat("Speed", 0.0f);
        
    }

    public void OnEnemyAttackEnd()
    {
        m_animator.SetBool("IsAttacking", false);
    }

    public void OnEndOfDamageAnim()
    {
        m_animator.SetBool("IsReceivingDamage", false);
    }
}
