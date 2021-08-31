using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private List<Color> m_ColorList = new List<Color>();

    private void Awake()
    {
        m_ColorList.Add(Resources.Load<Material>("FreeFurnitureSet/Materials/Wood").color);
        m_ColorList.Add(Color.red);
        m_ColorList.Add(Color.blue);
    }

    public void ChangeColor(GameObject gameObject, int colorIndex)
    {
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", m_ColorList[colorIndex - 1]);
    }
}
