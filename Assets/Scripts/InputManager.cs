using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    TelemetryManager tm;
    EnvironmentManager em;

    // Start is called before the first frame update
    void Start()
    {
        tm = FindObjectOfType<TelemetryManager>();
        em = GetComponent<EnvironmentManager>();
        this.SetupWatcher();
    }

    // Update is called once per frame
    void Update()
    {
        if (!File.Exists("Assets/Resources/rover-ui/rover-ui/comm_files/rover_target.json"))
        {
            this.ReloadTarget();
        }
    }

    private void SetupWatcher()
    {
        FileSystemWatcher watcher = new FileSystemWatcher("Assets/Resources/rover-ui/rover-ui/comm_files", "rover_telemetry.json");
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Debug.Log("CHANGED");
        tm.UpdateRover();
        em.UnloadPrevious();
        em.LoadEnvironment();
    }

    private void ReloadTarget()
    {
        tm.AddTargetIfAvailable();
    }

}
