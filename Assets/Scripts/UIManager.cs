using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    TelemetryManager tm;
    CameraMotion cm;

    private Transform batteryUI;
    private Transform velocityUI;
    private Transform stateUI;
    private Transform rotationUI;

    private Vector3 rot;


    public Vector3 GetRotation()
    {
        Slider sliderX = rotationUI.GetChild(5).GetComponent<Slider>();
        Slider sliderY = rotationUI.GetChild(6).GetComponent<Slider>();
        Slider sliderZ = rotationUI.GetChild(7).GetComponent<Slider>();
        return new Vector3(sliderX.value, sliderY.value, sliderZ.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TelemetryManager>();
        cm = FindObjectOfType<CameraMotion>();
        batteryUI = transform.GetChild(1);
        velocityUI = transform.GetChild(2);
        stateUI = transform.GetChild(3);
        rotationUI = transform.GetChild(4);
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
        rotationUI.GetChild(9).GetComponent<Text>().text = "X: " + Mathf.Round(rot.x);
        rotationUI.GetChild(10).GetComponent<Text>().text = "Y: " + Mathf.Round(rot.y);
        rotationUI.GetChild(11).GetComponent<Text>().text = "Z: " + Mathf.Round(rot.z);
    }

}
