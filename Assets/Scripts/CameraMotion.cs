using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    Camera mainCam;
    TelemetryManager tm;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCam = this.GetComponent<Camera>();
        tm = FindObjectOfType<TelemetryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaceNewTarget()
    {
        if(Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hitInfo))
        {
            tm.AddTarget(hitInfo.point);
        }
    }
}
