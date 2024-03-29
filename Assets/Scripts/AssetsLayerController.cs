// Reorders the characters' layer based on their Y position so that the closest character to the camera is in front of the other one.

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AssetsLayerController : MonoBehaviour
{
    Transform m_topLeftColumn;
    Transform m_bottomLeftColumn;
    SpriteRenderer m_playerSpriteRenderer;
    SpriteRenderer m_enemySpriteRenderer;

    private readonly int m_firstLayer = 1;
    private readonly int m_secondLayer = 2;
    private readonly int m_aboveAllColumns = 0;
    private readonly int m_bothCharactersOnSameLevel = 0;
    private readonly int m_exceptThisCharacter = 2;
    private readonly int m_betweenColumns = 3;
    private readonly int m_belowAllColumns = 6;


    private void Awake()
    {
        GameObject charactersGameObject = GameObject.Find("Characters");
        GameObject tilemapsGameObject = GameObject.Find("Tilemaps");
        Transform player = charactersGameObject.transform.Find("Player");
        Transform playerSprite = player.transform.Find("Sprite");
        m_playerSpriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        Transform enemy = charactersGameObject.transform.Find("Enemy");
        Transform enemySprite = enemy.transform.Find("Sprite");
        m_enemySpriteRenderer = enemySprite.GetComponent<SpriteRenderer>();
        m_topLeftColumn = tilemapsGameObject.transform.Find("TopLeftColumn").GetComponent<Transform>();
        m_bottomLeftColumn = tilemapsGameObject.transform.Find("BottomLeftColumn").GetComponent<Transform>();
    }

    void Update()
    {
        if (!m_playerSpriteRenderer)
        {
            return;
        }

        if (!m_enemySpriteRenderer)
        {
            ReorderPlayer();
            return;
        }

        ReorderBothCharacters();
    }

    private void ReorderPlayer()
    {
        if (m_playerSpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer;
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer + m_betweenColumns;
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y < m_bottomLeftColumn.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer + m_belowAllColumns;
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y < m_topLeftColumn.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer + m_betweenColumns;
            return;
        }

        m_playerSpriteRenderer.sortingOrder = m_firstLayer + m_betweenColumns;
    }

    private void ReorderBothCharacters()
    {
        if (m_playerSpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_aboveAllColumns, m_bothCharactersOnSameLevel);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y < m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_aboveAllColumns, m_exceptThisCharacter);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y < m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_aboveAllColumns, m_exceptThisCharacter);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_betweenColumns, m_bothCharactersOnSameLevel);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y < m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_betweenColumns, m_exceptThisCharacter);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y < m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters(m_betweenColumns, m_exceptThisCharacter);
            return;
        }

        ReorderCharacters(m_belowAllColumns, m_bothCharactersOnSameLevel);
    }

    private void ReorderCharacters(int layer, int layer2)
    {
        if (m_playerSpriteRenderer.transform.position.y > m_enemySpriteRenderer.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer + layer;
            m_enemySpriteRenderer.sortingOrder = m_secondLayer + layer + layer2;
        }
        else
        {
            m_playerSpriteRenderer.sortingOrder = m_secondLayer + layer + layer2;
            m_enemySpriteRenderer.sortingOrder = m_firstLayer + layer;
        }
    }
}
