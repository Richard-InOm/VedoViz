using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentObjects
{
    public EnvironmentObj[] environment;
}

    [System.Serializable]
public class EnvironmentObj
{ 
    public int type;
    public Vector3 size;
    public Vector3 position;
    public Vector3 rotation;

    private static GameObject[] possEnvObjs;

    public static void SetEnvObjs(GameObject[] envObjs)
    {
        EnvironmentObj.possEnvObjs = envObjs;
    }

    public GameObject AddToScene()
    {
        GameObject envObj = GameObject.Instantiate(EnvironmentObj.possEnvObjs[this.type], this.position, Quaternion.Euler(this.rotation));
        envObj.transform.localScale = this.size;
        return envObj;
    }

}

public class EnvironmentManager : MonoBehaviour
{

    public GameObject[] envObjs;

    private List<GameObject> currLoaded;

    // Start is called before the first frame update
    void Start()
    {
        currLoaded = new List<GameObject>();
        EnvironmentObj.SetEnvObjs(envObjs);
        LoadEnvironment();
    }

    public void UnloadPrevious()
    {
        foreach(GameObject loaded in currLoaded)
        {
            Destroy(loaded);
        }
    }

    public void LoadEnvironment()
    {
        TextAsset envJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_environment");
        Debug.Log(envJson.text);
        EnvironmentObjects objs = JsonUtility.FromJson<EnvironmentObjects>("{ \"environment\": " + envJson.text + " }");
        foreach (EnvironmentObj obj in objs.environment)
        {
            GameObject loaded = obj.AddToScene();
            currLoaded.Add(loaded);
        }
    }
}
