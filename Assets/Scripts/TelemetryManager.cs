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

    public void AddTarget(Vector3 pos)
    {
        TextAsset telemJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_telemetry");
        currTelem = JsonUtility.FromJson<Telemetry>(telemJson.text);
        Target[] currTargs = currTelem.targets;
        Target newTarg = new Target(currTargs.Length, pos, 1);

        //Converts Target to Json and writes to rover_target.json
        string targJson = JsonUtility.ToJson(newTarg);
        StreamWriter outputFile = new StreamWriter("rover-ui/rover-ui/comm_files/rover_target.json");
        outputFile.WriteLine(targJson);
        outputFile.Close();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // FixedUpdate is called 1/60th of a second
    void FixedUpdate()
    {
        UpdateRover();
    }

    private void UpdateRover()
    {
        TextAsset telemJson = Resources.Load<TextAsset>("rover-ui/rover-ui/comm_files/rover_telemetry");
        currTelem = JsonUtility.FromJson<Telemetry>(telemJson.text);
        this.transform.position = currTelem.position;
        this.transform.rotation = Quaternion.Euler(currTelem.rotation);
    }
}
