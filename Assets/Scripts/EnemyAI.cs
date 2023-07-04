// Source : https://www.youtube.com/watch?v=2xaQYZW6LLA

using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private LayerMask m_playerLayerMask;
    private Transform m_playerTransform;
    private Transform m_enemyTransform;
    private Transform m_charactersTransfom;
    private Rigidbody2D m_enemyRigidbody2D;
    private Animator m_animator;
    private Vector2 m_movement;
    private Vector3 m_direction;
    private float m_enemySpeed = 300.0f;
    private float m_checkRadius = 8.0f;
    private float m_attackRadius = 2.0f;
    private bool m_isInChaseRange = false;
    private bool m_isInAttackRange = false;
    //private bool m_isAttacking;

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
        m_animator.SetBool("IsEnemyWalking", m_isInChaseRange);
        
        m_isInChaseRange = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_playerLayerMask);
        m_isInAttackRange = Physics2D.OverlapCircle(transform.position, m_attackRadius, m_playerLayerMask);

        m_direction = m_playerTransform.position - transform.position;

        float angleToPlayer = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        m_direction.Normalize();
        m_movement = m_direction;

        if (m_direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        // Source : https://docs.unity3d.com/ScriptReference/Animator.GetCurrentAnimatorStateInfo.html
        if (m_isInAttackRange && !m_animator.GetBool("IsEnemyAttacking") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            Debug.Log("Ennemy is attacking");
            //m_isAttacking = true;
            m_animator.SetBool("IsEnemyAttacking", true);
            return;
        }
    }

    private void FixedUpdate()
    {
        if(m_isInChaseRange && !m_isInAttackRange)
        {
            Debug.Log("Ennemy is chasing");
            MoveEnemy(m_movement);
            return;
        }

        m_enemyRigidbody2D.velocity = Vector2.zero;
    }

    private void MoveEnemy(Vector2 movement)
    {
        m_enemyRigidbody2D.velocity = movement * m_enemySpeed * Time.fixedDeltaTime;
    }

    public void OnEnemyAttackEnd()
    {
        Debug.Log("OnEnemyAttackEnd");
        //m_isAttacking = false;
        m_animator.SetBool("IsEnemyAttacking", false);
        Debug.Log("IsEnemyAttacking: " + m_animator.GetBool("IsEnemyAttacking"));
    }
}
