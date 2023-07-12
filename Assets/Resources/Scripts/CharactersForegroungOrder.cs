// Reorders the characters' layer based on their Y position so that the closest character to the camera is in front of the other one.

using UnityEngine;

public class CharactersForegroungOrder : MonoBehaviour
{
    SpriteRenderer m_playerSpriteRenderer;
    SpriteRenderer m_enemySpriteRenderer;

    private void Awake()
    {
        m_playerSpriteRenderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        m_enemySpriteRenderer = GameObject.Find("Enemy").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (m_playerSpriteRenderer.transform.position.y > m_enemySpriteRenderer.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = 1;
            m_enemySpriteRenderer.sortingOrder = 2;
        }
        else
        {
            m_playerSpriteRenderer.sortingOrder = 2;
            m_enemySpriteRenderer.sortingOrder = 1;
        }
    }
}
