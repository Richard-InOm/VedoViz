using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TelemetryManager tm;

    private Transform batteryUI;
    private Transform velocityUI;
    private Transform stateUI;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TelemetryManager>();
        batteryUI = transform.GetChild(1);
        velocityUI = transform.GetChild(2);
        stateUI = transform.GetChild(3);
    }

    // Update is called once per frame
    void Update()
    {
        Telemetry telem = tm.GetTelemetry();
        UpdateBatteryUI(telem.batteryLevel);
        UpdateVelocityUI(telem.velocity);
        UpdateStateUI(telem.state);
    }

    private void UpdateBatteryUI(float batteryLevel)
    {
        batteryUI.GetComponentInChildren<Slider>().value = batteryLevel;
        batteryUI.GetComponentInChildren<Text>().text = "Battery Level: " + batteryLevel + "%";
    }

    private void UpdateVelocityUI(float velocity) {
        velocityUI.GetComponentInChildren<Text>().text = "Velocity: " + velocity + "m/s";
    }

    private void UpdateStateUI(int state) {
        Text myText = stateUI.GetComponentInChildren<Text>();
        if (state == 0) {
            myText.text = "State: Idle";
            myText.color = Color.yellow;
        }
        if (state == 1) {
            myText.text = "State: Moving";
            myText.color = Color.green;
        }
        if (state == -1) {
            myText.text = "State: Error";
            myText.color = Color.red;
        }
    }
}
