using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Telemetry
{
    public Vector3 position;
    public Vector3 rotation;
    public float batteryLevel;
    public int state;
    public float velocity;
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
        this.position = new Vector3(this.position.x, this.position.z, this.position.y);
        this.action = act;
    }
}

[System.Serializable]
public class ActionJson
{
    public int action;

    public ActionJson(int act)
    {
        this.action = act;
    }
}

public class TelemetryManager : MonoBehaviour
{
    public GameObject telemetryObj;
    public Telemetry currTelem;
    private List<Target> potentialTargs;
    private List<GameObject> potentialTargMarks;

    private List<Target> actualTargs;
    private List<GameObject> targMarks;
    private GameObject currObj;
    private bool updateNow;

    public GameObject targMarker;

    private void Start()
    {
        UpdateJson();
        potentialTargs = new List<Target>();
        potentialTargMarks = new List<GameObject>();
        actualTargs = new List<Target>();
        targMarks = new List<GameObject>();
    }

    private void Update()
    {
        if (updateNow)
        {
            UpdatePos();
        }
    }

    public Telemetry GetTelemetry()
    {
        return currTelem;
    }

    public Vector3 GetRoverPos()
    {
        return currObj.transform.position;
    }

    public void SpawnTarget(Vector3 pos)
    {
        Target newTarg = new Target(0, pos, 1);
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

    public void UpdateJson()
    {
        Debug.Log("UPDATING ROVER");
        StreamReader reader = new StreamReader("Assets/Resources/rover-ui/rover-ui/comm_files/rover_telemetry.json");
        string telemJson = reader.ReadToEnd();
        reader.Close();
        currTelem = JsonUtility.FromJson<Telemetry>(telemJson);
        Debug.Log(telemJson);
        updateNow = true;
    }

    public void UpdatePos()
    {
        Vector3 pos = new Vector3(currTelem.position.x, currTelem.position.z, currTelem.position.y);
        if (currObj == null)
        {
            currObj = GameObject.Instantiate(telemetryObj, pos, Quaternion.Euler(currTelem.rotation));
        }
        else
        {
            currObj.transform.SetPositionAndRotation(pos, Quaternion.Euler(currTelem.rotation));
        }
    }

    public void EmergencyStop()
    {
        //Converts Target to Json and writes to rover_action.json
        string actJson = JsonUtility.ToJson(new ActionJson(2), true);

        StreamWriter outputFile = new StreamWriter("Assets/Resources/rover-ui/rover-ui/comm_files/rover_action.json");
        outputFile.WriteLine(actJson);
        outputFile.Close();
    }

}
