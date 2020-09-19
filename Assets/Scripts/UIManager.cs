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
    }

    // Update is called once per frame
    void Update()
    {
        Telemetry telem = tm.GetTelemetry();
        UpdateBatteryUI(telem.batteryLevel);
    }

    private void UpdateBatteryUI(float batteryLevel)
    {
        batteryUI.GetComponentInChildren<Slider>().value = batteryLevel;
        batteryUI.GetComponentInChildren<Text>().text = "Battery Level: " + batteryLevel + "%";
    }
}
