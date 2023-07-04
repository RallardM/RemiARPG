// Source : https://www.youtube.com/watch?v=2xaQYZW6LLA

using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float m_enemySpeed = 500.0f;
    public float m_checkRadius = 1.0f;
    public float m_attackRadius = 1.0f;

    public bool m_shouldRotate = true;

    private LayerMask m_playerLayerMask;
    private Transform m_playerTransform;
    private Transform m_enemyTransform;
    private Transform m_charactersTransfom;
    private Rigidbody2D m_enemyRigidbody2D;
    private Animator m_animator;
    private Vector2 m_movement;
    public Vector3 m_direction;

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
    }

    private void Update()
    {
        m_animator.SetBool("CanWalk", m_isInChaseRange);
        
        m_isInChaseRange = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_playerLayerMask);
        m_isInAttackRange = Physics2D.OverlapCircle(transform.position, m_attackRadius, m_playerLayerMask);

        m_direction = m_playerTransform.position - transform.position;

        float angleToPlayer = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        m_direction.Normalize();
        m_movement = m_direction;

        if(m_shouldRotate)
        {
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, angleToPlayer);
            m_animator.SetFloat("X", m_movement.x);
            m_animator.SetFloat("Y", m_movement.y);
        }
    }

    private void FixedUpdate()
    {
        if(m_isInChaseRange && !m_isInAttackRange)
        {
            MoveEnemy(m_movement);
        }
        else if (m_isInAttackRange)
        {
            //m_animator.SetTrigger("Attack");
            m_enemyRigidbody2D.velocity = Vector2.zero;
        }
        else
        {
            m_animator.SetBool("CanWalk", false);
        }
    }

    private void MoveEnemy(Vector2 m_movement)
    {
        m_enemyRigidbody2D.MovePosition((Vector2)transform.position + (m_movement * m_enemySpeed * Time.deltaTime));
    }
}
