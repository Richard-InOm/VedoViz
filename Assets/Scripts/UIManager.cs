using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TelemetryManager tm;

    private Transform batteryUI;

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

    private void UpdateVelocityUI(int state) {
        if (state == 0) {
            stateUI.GetComponentInChildren<Text>().text = "State: Idle";
            GetComponent<Text>().color = Color.yellow;
        }
        if (state == 1) {
            stateUI.GetComponentInChildren<Text>().text = "State: Moving";
            GetComponent<Text>().color = Color.green;
        }
        if (state == -1) {
            stateUI.GetComponentInChildren<Text>().text = "State: Error";
            GetComponent<Text>().color = Color.red;
        }
    }
}
