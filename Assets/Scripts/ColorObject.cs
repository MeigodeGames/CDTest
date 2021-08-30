using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private List<Color> m_ColorList = new List<Color> { Color.red, Color.green, Color.blue };

    private void Awake()
    {
        Color original = Resources.Load<Material>("FreeFurnitureSet/Materials/Wood").color;
        m_ColorList.Add(original);
    }

    public void ChangeColor(GameObject gameObject, int colorIndex)
    {
        gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", m_ColorList[colorIndex - 1]);
    }
}
