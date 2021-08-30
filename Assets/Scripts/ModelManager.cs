using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    private string m_URL = "https://s3-sa-east-1.amazonaws.com/static-files-prod/unity3d/models.json";
    private GameObject[] m_ModelList;

    public List<GameObject> m_ObjectList;

    [System.Serializable]
    public class RawModel
    {
        public string name { get; set; }
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public float[] scale { get; set; }
    }

    [System.Serializable]
    public class RawList
    {
        public List<RawModel> models { get; set; }
    }

    private void Awake()
    {
        m_ModelList = Resources.LoadAll<GameObject>("FreeFurnitureSet/Prefabs");
        m_ObjectList = new List<GameObject>();
    }

    void Start()
    {
        NewList();
    }
        
    public async void NewList()
    {
        var result = await new HttpClient().GetStringAsync(m_URL);
        var rawList = JsonConvert.DeserializeObject<RawList>(result);

        CreateModels(rawList);
    }

    private void CreateModels(RawList rawList)
    {
        int i = 0;
        foreach (RawModel model in rawList.models)
        {
            Vector3 position = new Vector3(model.position[0], model.position[1], model.position[2]);
            Vector3 rotation = new Vector3(model.rotation[0], model.rotation[1], model.rotation[2]);
            Vector3 scale = new Vector3(model.scale[0], model.scale[1], model.scale[2]);

            GameObject gameObject = new GameObject(model.name);
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
            gameObject.transform.localScale = scale;

            Instantiate(m_ModelList[i++], gameObject.transform);
            if (i == m_ModelList.Length) i = 0;

            m_ObjectList.Add(gameObject);
        }
    }

    /*
    private void CreateModels(ObjectList objectList)
    {
    }
    */

    /*
    public void LoadList()
    {
        CreateModels(objectList);
    }
    */
}
