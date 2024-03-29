// The HitBox is assigned to both the player and the enemy. It is used to detect when any of them is hit by an attack.
using UnityEngine;

public class HitBox : MonoBehaviour
{
    LayerMask m_selfLayerMask;
    Transform m_characterTransform;
    GameObject m_characterGameObject;
    Rigidbody2D m_hitBoxRigidbody;
    Animator m_chatacterAnimator;
    private float m_health = 100.0f;
    private float m_damage = 10.0f;

    public float GetHealth { get{return m_health;} }

    private void Awake()
    {
        m_selfLayerMask = gameObject.layer;
        m_characterTransform = transform.parent;
        m_characterGameObject = transform.parent.gameObject;
        m_hitBoxRigidbody = GetComponent<Rigidbody2D>();
        Transform character = transform.parent;
        GameObject Sprite = character.Find("Sprite").gameObject;
        m_chatacterAnimator = Sprite.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Keeps the character's hitbox from moving while the character is dead.
        if (m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        UpdatePosition();
    }

    private void Update()
    {
        // Keeps the character's hitbox from animating while the character is dead.
        if (m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        CheckIfDead();
    }

    private void CheckIfDead()
    {
        if (this.m_health <= 0.0f)
        {
            m_chatacterAnimator.SetBool("IsDying", true);
        }
    }

    private void UpdatePosition()
    {
        // Update the hitbox's position to match the character's position.
        float horizontal = m_characterTransform.localPosition.x;
        float vertical = m_characterTransform.localPosition.y;
        Vector2 movement = new(horizontal, vertical);
        m_hitBoxRigidbody.MovePosition(movement);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Keeps the character's hitbox from receiving damage while the character is dead.
        if(m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        // Removes health from the character if the hitbox collides while the oponent is attacking only.
        if (m_characterGameObject != other.gameObject.transform.parent.parent.gameObject && other.gameObject.transform.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            m_chatacterAnimator.SetBool("IsReceivingDamage", false);
            m_chatacterAnimator.Rebind();
            m_health -= m_damage;
            m_chatacterAnimator.SetBool("IsReceivingDamage", true);
        }
    }
}
