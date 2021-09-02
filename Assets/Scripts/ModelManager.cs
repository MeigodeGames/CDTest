using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    private string m_URL = "https://s3-sa-east-1.amazonaws.com/static-files-prod/unity3d/models.json";
    private List<GameObject> m_ObjectList;

    [System.Serializable]
    public class RawList
    {
        public List<RawModel> models { get; set; }
    }
    [System.Serializable]
    public class RawModel
    {
        public string name { get; set; }
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public float[] scale { get; set; }
    }

    [System.Serializable]
    public class UserList
    {
        public List<UserModel> models { get; set; }
    }
    [System.Serializable]
    public class UserModel
    {
        public string name { get; set; }
        public float[] position { get; set; }
        public float[] rotation { get; set; }
        public float[] scale { get; set; }
        public string modelName { get; set; }
        public string modelColor { get; set; }
        public string modelTexture { get; set; }
    }

    private void Awake()
    {
        m_ObjectList = new List<GameObject>();
    }

    [ContextMenu("New")]
    public async void NewScene()
    {
        string result = await new HttpClient().GetStringAsync(m_URL);
        RawList rawList = JsonConvert.DeserializeObject<RawList>(result.ToString());

        CreateModels(rawList);
    }

    [ContextMenu("Load")]
    public void LoadScene()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        string destination = Application.persistentDataPath + "/save.json";

        if (!File.Exists(destination))
        {
            Debug.Log("Sem arquivo salvo para carregar");
            return;
        }

        string jsonText = File.ReadAllText(destination);
        UserList userList = JsonConvert.DeserializeObject<UserList>(File.ReadAllText(destination));
        CreateModels(userList);
    }

    public void AddModel(GameObject newModel)
    {
        m_ObjectList.Add(newModel);
    }

    [ContextMenu("Save")]
    public void SaveScene()
    {
        UserList jsonList = ConvertToUserList(m_ObjectList);
        string destination = Application.persistentDataPath + "/save.json";
        string jsonText = JsonConvert.SerializeObject(jsonList, Formatting.Indented);

        File.WriteAllText(destination, jsonText);
    }

    private UserList ConvertToUserList(List<GameObject> ObjectList)
    {
        UserList userList = new UserList();
        userList.models = new List<UserModel>();

        foreach (GameObject gameObject in m_ObjectList)
        {
            UserModel userModel = new UserModel();
            userModel.name = gameObject.name;

            userModel.position = new float[3];
            userModel.position[0] = gameObject.transform.position.x;
            userModel.position[1] = gameObject.transform.position.y;
            userModel.position[2] = gameObject.transform.position.z;

            userModel.rotation = new float[3];
            userModel.rotation[0] = gameObject.transform.eulerAngles.x;
            userModel.rotation[1] = gameObject.transform.eulerAngles.y;
            userModel.rotation[2] = gameObject.transform.eulerAngles.z;

            userModel.scale = new float[3];
            userModel.scale[0] = gameObject.transform.localScale.x;
            userModel.scale[1] = gameObject.transform.localScale.y;
            userModel.scale[2] = gameObject.transform.localScale.z;

            userModel.modelName = gameObject.GetComponentInChildren<MeshFilter>().sharedMesh.name;
            userModel.modelColor = "#" + ColorUtility.ToHtmlStringRGB(gameObject.GetComponentInChildren<Renderer>().material.color);
            userModel.modelTexture = AssetDatabase.GetAssetPath(gameObject.GetComponentInChildren<Renderer>().material.mainTexture);

            userList.models.Add(userModel);
        }

        return userList;
    }

    private void CreateModels(RawList rawList)
    {
        GameObject[] m_ModelList = Resources.LoadAll<GameObject>("FreeFurnitureSet/Prefabs");
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

            gameObject.transform.SetParent(transform);

            Instantiate(m_ModelList[i++], gameObject.transform);
            if (i == m_ModelList.Length) i = 0;

            gameObject.AddComponent<BoxCollider>();
            m_ObjectList.Add(gameObject);
        }
    }

    private void CreateModels(UserList userList)
    {
        foreach (UserModel model in userList.models)
        {
            Vector3 position = new Vector3(model.position[0], model.position[1], model.position[2]);
            Vector3 rotation = new Vector3(model.rotation[0], model.rotation[1], model.rotation[2]);
            Vector3 scale = new Vector3(model.scale[0], model.scale[1], model.scale[2]);

            if (!ColorUtility.TryParseHtmlString(model.modelColor, out Color color))
            {
                Debug.Log("Falha carregando cor");
            }

            string textureAsset = (Path.ChangeExtension(model.modelTexture, null)).Replace("Assets/Resources/", "");
            Texture texture = Resources.Load<Texture>(textureAsset);

            GameObject gameObject = new GameObject(model.name);
            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;
            gameObject.transform.localScale = scale;

            gameObject.transform.SetParent(transform);


            GameObject prefab = Resources.Load<GameObject>("FreeFurnitureSet/Prefabs/" + model.modelName);
            Instantiate(prefab, gameObject.transform);

            gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", color);
            gameObject.GetComponentInChildren<Renderer>().material.SetTexture("_MainTex", texture);

            gameObject.AddComponent<BoxCollider>();
            m_ObjectList.Add(gameObject);
        }
    }

}
