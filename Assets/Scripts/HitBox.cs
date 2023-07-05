using UnityEngine;

public class HitBox : MonoBehaviour
{
    LayerMask m_selfLayerMask;
    Transform m_characterTransform;
    GameObject m_characterGameObject;
    Rigidbody2D m_hitBoxRigidbody;
    Animator m_chatacterAnimator;
    private float m_health = 100.0f;

    private void Awake()
    {
        m_selfLayerMask = gameObject.layer;
        m_characterTransform = transform.parent;
        m_characterGameObject = transform.parent.gameObject;
        m_hitBoxRigidbody = GetComponent<Rigidbody2D>();
        m_chatacterAnimator = transform.parent.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        float horizontal = m_characterTransform.localPosition.x;
        float vertical = m_characterTransform.localPosition.y;
        Vector2 movement = new(horizontal, vertical);
        m_hitBoxRigidbody.MovePosition(movement);
    }

    private void Update()
    {
        if (m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        if (this.m_health <= 0.0f)
        {
            m_chatacterAnimator.SetBool("IsDying", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(m_chatacterAnimator.GetBool("IsDying"))
        {
            return;
        }

        if (m_characterGameObject != other.gameObject.transform.parent.parent.gameObject)
        {
            m_chatacterAnimator.SetBool("IsReceivingDamage", false);
            m_chatacterAnimator.Rebind();
            m_health -= 10.0f;
            m_chatacterAnimator.SetBool("IsReceivingDamage", true);
        }
    }
}
