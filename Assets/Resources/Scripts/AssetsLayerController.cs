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
        m_playerSpriteRenderer = charactersGameObject.transform.Find("Player").GetComponent<SpriteRenderer>();
        m_enemySpriteRenderer = charactersGameObject.transform.Find("Enemy").GetComponent<SpriteRenderer>();
        m_topLeftColumn = tilemapsGameObject.transform.Find("TopLeftColumn").GetComponent<Transform>();
        m_bottomLeftColumn = tilemapsGameObject.transform.Find("BottomLeftColumn").GetComponent<Transform>();
    }

    void Update()
    {
        
        if (m_playerSpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_aboveAllColumns, m_bothCharactersOnSameLevel);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y < m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_aboveAllColumns, m_exceptThisCharacter);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y < m_topLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_topLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_aboveAllColumns, m_exceptThisCharacter);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_betweenColumns, m_bothCharactersOnSameLevel);
            return;
        }

        if (m_playerSpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y < m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_betweenColumns, m_exceptThisCharacter);
            return;
        }

        var playerPosition = m_playerSpriteRenderer.transform.position.y;
        var enemyPosition = m_enemySpriteRenderer.transform.position.y;
        var leftColumnPosition = m_bottomLeftColumn.transform.position.y;
        bool playerIsNotAboveLeftColumn = playerPosition !>= leftColumnPosition;
        bool enemyIsNotAboveLeftColumn = enemyPosition !>= leftColumnPosition;
        if (m_playerSpriteRenderer.transform.position.y < m_bottomLeftColumn.transform.position.y && m_enemySpriteRenderer.transform.position.y > m_bottomLeftColumn.transform.position.y)
        {
            ReorderCharacters2(m_betweenColumns, m_exceptThisCharacter);
            return;
        }

        ReorderCharacters2(m_belowAllColumns, m_bothCharactersOnSameLevel);
    }

    private void ReorderCharacters(int layer)
    {
        if (m_playerSpriteRenderer.transform.position.y > m_enemySpriteRenderer.transform.position.y)
        {
            m_playerSpriteRenderer.sortingOrder = m_firstLayer + layer;
            m_enemySpriteRenderer.sortingOrder = m_secondLayer + layer;
        }
        else
        {
            m_playerSpriteRenderer.sortingOrder = m_secondLayer + layer;
            m_enemySpriteRenderer.sortingOrder = m_firstLayer + layer;
        }
    }

    private void ReorderCharacters2(int layer, int layer2)
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
