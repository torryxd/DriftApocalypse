using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorFader : MonoBehaviour
{
    public SpriteRenderer spr;
    public Color[] m_Colors;
    private Color currentColour;
    private int colorIndex = 0;

    void Start()
    {
        if (m_Colors.Length > 0)
        {
            currentColour = m_Colors[0];
        }
    }

    void Update()
    {
        for (int i = 0; i < m_Colors.Length; i++)
        {
            // Get the currentColor in the Array
            if (currentColour == m_Colors[i])
            {
                colorIndex = i + 1 == m_Colors.Length ? 0 : i + 1;
            }
        }
        Color nextColor = m_Colors[colorIndex];
        // Lerp Color _>
        currentColour = Color.Lerp(currentColour, nextColor, Time.deltaTime * 15);
        spr.color = currentColour;
    }
}
