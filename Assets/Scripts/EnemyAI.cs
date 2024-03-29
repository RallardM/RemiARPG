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
    private Transform m_enemyHitBox;
    private Animator m_enemyAnimator;
    private Animator m_playerAnimator;
    private SpriteRenderer m_enemySpriteRenderer;

    private Vector2 m_movement;
    private Vector3 m_direction;
    private Vector3 m_invertVector3X = new(-0.1f, 0.1f, 0.1f);
    private Vector3 m_revertVector3X = new(0.1f, 0.1f, 0.1f);
    private readonly float m_enemySpeed = 300.0f;
    private readonly float m_checkRadius = 9.0f;
    private readonly float m_attackRadius = 2.0f;
    private bool m_isInChaseRange = false;
    private bool m_isInAttackRange = false;

    private void Awake()
    {
        m_enemyRigidbody2D = GetComponent<Rigidbody2D>();
        Transform enemySprite = GetComponentInChildren<Transform>().Find("Sprite");
        m_enemySpriteRenderer = enemySprite.GetComponent<SpriteRenderer>();
        m_enemyAnimator = enemySprite.GetComponent<Animator>();
        m_enemyTransform = transform;
        m_charactersTransfom = m_enemyTransform.transform.parent;
        m_playerTransform = m_charactersTransfom.Find("Player");
        Transform playerSprite = m_playerTransform.GetComponentInChildren<Transform>().Find("Sprite");
        m_playerAnimator = playerSprite.GetComponent<Animator>();
        m_playerLayerMask = LayerMask.GetMask("Player");
        m_enemySword = GetComponentInChildren<Transform>().Find("Sprite/EnemySword");
        m_enemyHitBox = m_enemyTransform.GetComponentInChildren<Transform>().Find("EnemyHitBox");
    }

    private void Update()
    {
        // Returns if the player gameobject is destroyed
        if (!m_playerAnimator)
        {
            return;
        }

        // Returns if the enemy gameobject is destroyed
        if (!m_enemyAnimator)
        {
            return;
        }

        // Keeps the enemy from moving once he's dead.
        if (m_enemyAnimator.GetBool("IsDying") == true)
        {
            m_enemyRigidbody2D.velocity = Vector2.zero;
            return;
        }

        // Keeps the enemy from playing his walking animation when the player is dead.
        if (m_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
        {
            m_enemyAnimator.SetBool("IsWalking", false);
            return;
        }

        AnimEnemy();
    }

    private void FixedUpdate()
    {
        // Returns if the player gameobject is destroyed
        if (!m_playerAnimator)
        {
            return;
        }

        // Returns if the enemy gameobject is destroyed
        if (!m_enemyAnimator)
        {
            return;
        }

        // Keeps the enemy from moving when the state of the player player is IsDying.
        if (m_enemyAnimator.GetBool("IsDying") || m_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dying"))
        {
            m_enemyRigidbody2D.velocity = Vector2.zero;
            return;
        }

        MoveEnemy();
    }

    private void AnimEnemy()
    {
        // Makes the enemy play his walking animation when the player is in his chase range.
        m_enemyAnimator.SetBool("IsWalking", m_isInChaseRange);

        UpdateCombatRanges();

        UpdateDirection();

        FlipEnemy();

        Attack();
    }

    private void Attack()
    {
        // Source : https://docs.unity3d.com/ScriptReference/Animator.GetCurrentAnimatorStateInfo.html
        // Makes the enemy play his attack animation when the player is in his attack range.
        if (m_isInAttackRange && !m_enemyAnimator.GetBool("IsAttacking") && !m_enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            m_enemyAnimator.SetBool("IsWalking", false);
            m_enemyAnimator.SetBool("IsAttacking", true);
            return;
        }
    }

    private void FlipEnemy()
    {
        // Conditions that flip the enemy sprite, second castShadow in hitbox and sword collider depending on the direction he's facing.
        if (m_direction.x < 0 && !m_enemyAnimator.GetBool("IsDying"))
        {
            m_enemySpriteRenderer.flipX = true;
            m_enemySword.localScale = new Vector3(transform.localScale.x * m_invertVector3X.x, transform.localScale.y * m_invertVector3X.y, transform.localScale.z * m_invertVector3X.z);
            m_enemyHitBox.localScale = new Vector3(transform.localScale.x * m_invertVector3X.x, transform.localScale.y * m_invertVector3X.y, transform.localScale.z * m_invertVector3X.z);
        }
        else if (m_direction.x > 0 && !m_enemyAnimator.GetBool("IsDying"))
        {
            m_enemySpriteRenderer.flipX = false;
            m_enemySword.localScale = new Vector3(transform.localScale.x * m_revertVector3X.x, transform.localScale.y * m_revertVector3X.y, transform.localScale.z * m_revertVector3X.z);
            m_enemyHitBox.localScale = new Vector3(transform.localScale.x * m_revertVector3X.x, transform.localScale.y * m_revertVector3X.y, transform.localScale.z * m_revertVector3X.z);
        }
    }

    private void UpdateDirection()
    {
        // Sets the enemy direction towards the player.
        m_direction = m_playerTransform.position - transform.position;
        m_direction.Normalize();
        // m_movement is later used as the vector2 velocity of the enemy.
        m_movement = m_direction;
    }

    private void UpdateCombatRanges()
    {
        // Source : https://youtu.be/2xaQYZW6LLA
        // Sets the ranges that activate the combat states.
        m_isInChaseRange = Physics2D.OverlapCircle(transform.position, m_checkRadius, m_playerLayerMask);
        m_isInAttackRange = Physics2D.OverlapCircle(transform.position, m_attackRadius, m_playerLayerMask);
    }

    private void MoveEnemy()
    {
        // When the player is in the enemy's chase range, the enemy moves towards the player.
        if (m_isInChaseRange && !m_isInAttackRange)
        {
            m_enemyRigidbody2D.velocity = m_enemySpeed * Time.fixedDeltaTime * m_movement;
            return;
        }
  
        m_enemyRigidbody2D.velocity = Vector2.zero;
    }
}
