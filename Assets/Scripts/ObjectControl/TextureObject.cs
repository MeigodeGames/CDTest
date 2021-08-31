using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureObject : MonoBehaviour
{
    private List<Texture> m_TextureList = new List<Texture>();

    void Awake()
    {
        m_TextureList.Add(Resources.Load<Texture>("FreeFurnitureSet/Textures/wood"));
        m_TextureList.Add(Resources.Load<Texture>("ADG_Textures/Plank Textures/Planks1/textures/Planks1a"));
        m_TextureList.Add(Resources.Load<Texture>("ADG_Textures/Plank Textures/Planks2/textures/Planks2a"));
    }

    public void ChangeTexture(GameObject gameObject, int colorIndex)
    {
        gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", m_TextureList[colorIndex - 1]);
    }
}
