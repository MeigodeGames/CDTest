using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ModelLoader : MonoBehaviour
{
    private string m_URL = "https://s3-sa-east-1.amazonaws.com/static-files-prod/unity3d/models.json";

    public Root m_ModelList;

    [System.Serializable]
    public class Model
    {
        public string name { get; set; }
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public float[] scale { get; set; }
    }

    [System.Serializable]
    public class Root
    {
        public List<Model> models { get; set; }
    }


    // Start is called before the first frame update
    void Start()
    {
        m_ModelList = new Root();
        GetModels();
    }

    public async void GetModels()
    {
        var result = await new HttpClient().GetStringAsync(m_URL);

        m_ModelList = JsonConvert.DeserializeObject<Root>(result);
        CreateModels();
    }

    private void CreateModels()
    {
        foreach (Model model in m_ModelList.models)
        {
            Vector3 position = new Vector3(model.position[0], model.position[1], model.position[2]);
            Vector3 rotation = new Vector3(model.rotation[0], model.rotation[1], model.rotation[2]);
            Vector3 scale = new Vector3(model.scale[0], model.scale[1], model.scale[2]);

            var gameObject = new GameObject(model.name);
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
            gameObject.transform.localScale = scale;

            /*
            Debug.Log(model.name);
            Debug.Log(position);
            Debug.Log(rotation);
            Debug.Log(scale);
            */
        }
    }
}
