using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Telemetry
{
    public Target position;
    public Vector3 rotation;
    public float batteryLevel;
    public int state;
    public float velocity;
    //will not be ints vvvv
    public Target[] targets;
}

[System.Serializable]
public class Target
{
    public int id;
    public Vector3 position;
    public int action;

    public Target(int id, Vector3 pos, int act)
    {
        this.id = id;
        this.position = pos;
        this.action = act;
    }
}

public class TelemetryManager : MonoBehaviour
{

    private Telemetry currTelem;
    private List<Target> potentialTargs;
    private List<GameObject> potentialTargMarks;

    private List<Target> actualTargs;
    private List<GameObject> targMarks;

    public GameObject targMarker;

    private void Start()
    {
        UpdateRover();
        potentialTargs = new List<Target>();
        potentialTargMarks = new List<GameObject>();
        actualTargs = new List<Target>();
        targMarks = new List<GameObject>();
    }

    public void SpawnTarget(Vector3 pos)
    {
        TextAsset telemJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_telemetry");
        currTelem = JsonUtility.FromJson<Telemetry>(telemJson.text);
        Target[] currTargs = currTelem.targets;

        Target newTarg = new Target(currTargs.Length, pos, 1);
        GameObject targMark = GameObject.Instantiate(targMarker, pos, Quaternion.identity);

        potentialTargs.Add(newTarg);
        potentialTargMarks.Add(targMark);
    }

    public void AddTargets()
    {
        foreach (Target targ in this.potentialTargs)
        {
            this.actualTargs.Add(targ);
        }
        potentialTargs = new List<Target>();

        foreach (GameObject targMark in this.potentialTargMarks)
        {
            this.targMarks.Add(targMark);
        }
        potentialTargMarks = new List<GameObject>();
    }

    public void AddTargetIfAvailable()
    {
        if (actualTargs.Count > 0)
        {
            //Converts Target to Json and writes to rover_target.json
            string targJson = JsonUtility.ToJson(actualTargs[0], true);
            actualTargs.RemoveAt(0);

            StreamWriter outputFile = new StreamWriter("Assets/Resources/rover-ui/rover-ui/comm_files/rover_target.json");
            outputFile.WriteLine(targJson);
            outputFile.Close();

            Destroy(targMarks[0]);
            targMarks.RemoveAt(0);
        }
    }

    public Telemetry GetTelemetry()
    {
        return currTelem;
    }

    public void UpdateRover()
    {
        Debug.Log("UPDATING ROVER");
        TextAsset telemJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_telemetry");
        Debug.Log("LOADED");
        currTelem = JsonUtility.FromJson<Telemetry>(telemJson.text);
        Debug.Log("PARSED");
        Debug.Log(telemJson.text);
        
        this.transform.position = currTelem.position.position;
        this.transform.rotation = Quaternion.Euler(currTelem.rotation);
    }

}
