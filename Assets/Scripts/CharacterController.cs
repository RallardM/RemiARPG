using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_animator.SetBool("CanWalk", true);
    }

    // Update is called once per frame
    void Update()
    {
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
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_spriteRenderer.flipX = false;
            m_animator.SetFloat("Speed", 1.0f);
            return;
        }


        m_animator.SetFloat("Speed", 0.0f);
    }
}
