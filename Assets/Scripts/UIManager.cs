﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class SettingsJson
{
    public float motorSpeed;
    public Vector3 cameraRotation;
    public Dictionary<string, string> others;
}

public class UIManager : MonoBehaviour
{
    TelemetryManager tm;
    CameraMotion cm;

    private Transform batteryUI;
    private Transform velocityUI;
    private Transform stateUI;
    private Transform rotationUI;

    private Vector3 rot, lastRot;


    public Vector3 GetRotation()
    {
        Slider sliderX = GameObject.FindGameObjectWithTag("sliderX").GetComponent<Slider>();
        Slider sliderY = GameObject.FindGameObjectWithTag("sliderY").GetComponent<Slider>();
        Slider sliderZ = GameObject.FindGameObjectWithTag("sliderZ").GetComponent<Slider>();
        return new Vector3(sliderX.value, sliderY.value, sliderZ.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TelemetryManager>();
        cm = FindObjectOfType<CameraMotion>();
        batteryUI = GameObject.FindGameObjectWithTag("batteryUI").transform;
        velocityUI = GameObject.FindGameObjectWithTag("velocityUI").transform;
        stateUI = GameObject.FindGameObjectWithTag("stateUI").transform;
        rotationUI = GameObject.FindGameObjectWithTag("rotationUI").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Telemetry telem = tm.GetTelemetry();
        UpdateBatteryUI(telem.batteryLevel);
        UpdateVelocityUI(telem.velocity);
        UpdateStateUI(telem.state);
        UpdateRotationUI();
    }

    private void UpdateBatteryUI(float batteryLevel)
    {
        batteryUI.GetComponentInChildren<Slider>().value = batteryLevel;
        batteryUI.GetComponentInChildren<Text>().text = "Battery Level: " + batteryLevel + "%";
    }

    private void UpdateVelocityUI(float velocity)
    {
        velocityUI.GetComponentInChildren<Text>().text = "Velocity: " + velocity + "m/s";
    }

    private void UpdateStateUI(int state)
    {
        Text myText = stateUI.GetComponentInChildren<Text>();
        if (state == 0) {
            myText.text = "State: Idle";
            myText.color = Color.yellow;
        }
        if (state == 1) {
            myText.text = "State: Moving";
            myText.color = Color.green;
        }
        if (state == 2) {
            myText.text = "State: Error";
            myText.color = Color.red;
        }
    }

    private void UpdateRotationUI()
    {
        Vector3 rot = cm.GetCamEulers();
        if (lastRot != rot)
        {
            GameObject.FindGameObjectWithTag("rotX").GetComponent<Text>().text = "X: " + Mathf.Round(rot.x);
            GameObject.FindGameObjectWithTag("rotY").GetComponent<Text>().text = "Y: " + Mathf.Round(rot.y);
            GameObject.FindGameObjectWithTag("rotZ").GetComponent<Text>().text = "Z: " + Mathf.Round(rot.z);
            lastRot = rot;

            //Reads rover_settings and overwrites the cameraRotation
            StreamReader reader = new StreamReader("Assets/Resources/rover-ui/rover-ui/comm_files/rover_settings.json");
            string settJson = reader.ReadToEnd();
            reader.Close();

            SettingsJson settings = JsonUtility.FromJson<SettingsJson>(settJson);

            settings.cameraRotation = rot;

            string myJson = JsonUtility.ToJson(settings, true);

            StreamWriter outputFile = new StreamWriter("Assets/Resources/rover-ui/rover-ui/comm_files/rover_settings.json");
            outputFile.WriteLine(myJson);
            outputFile.Close();

        }
    }

}
