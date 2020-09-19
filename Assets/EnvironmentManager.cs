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

    public void AddToScene()
    {
        GameObject envObj = GameObject.Instantiate(EnvironmentObj.possEnvObjs[this.type], this.position, Quaternion.Euler(this.rotation));
        //envObj.transform.localScale = this.size;
    }

}

public class EnvironmentManager : MonoBehaviour
{

    public GameObject[] envObjs;

    // Start is called before the first frame update
    void Start()
    {
        EnvironmentObj.SetEnvObjs(envObjs);

        TextAsset envJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_environment");
        Debug.Log(envJson.text);
        EnvironmentObjects objs = JsonUtility.FromJson<EnvironmentObjects>("{ \"environment\": " + envJson.text + " }");
        foreach (EnvironmentObj obj in objs.environment)
        {
            obj.AddToScene();
        }
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        
    }
}
