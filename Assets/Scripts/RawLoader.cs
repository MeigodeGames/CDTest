using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawLoader : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
