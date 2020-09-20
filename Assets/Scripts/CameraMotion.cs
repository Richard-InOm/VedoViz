using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    Camera mainCam;
    TelemetryManager tm;
    UIManager ui;
    float moveSpeed;
    float rotSpeed;
    bool placingTarget;

    public GameObject tooltip;

    // Start is called before the first frame update
    void Start()
    {
        this.mainCam = this.GetComponent<Camera>();
        tm = FindObjectOfType<TelemetryManager>();
        ui = FindObjectOfType<UIManager>();
        moveSpeed = 10f;
        rotSpeed = 2f;
        placingTarget = false;
    }

    // Update is called once per frame
    void Update()
    {

        float Xaxis = Input.GetAxis("MovementX");
        float Yaxis = Input.GetAxis("MovementY");
        float Zaxis = Input.GetAxis("MovementZ");
        transform.Translate(new Vector3(Xaxis, Yaxis, Zaxis) * Time.deltaTime * moveSpeed);
        Vector3 rotation = ui.GetRotation();
        transform.Rotate(rotation, Space.Self);

        if (placingTarget && Input.GetMouseButtonDown(0))
        {
            PlaceNewTarget();
            placingTarget = false;
            tooltip.SetActive(false);
        }
        
    }

    public void PreparePlacingTarget()
    {
        placingTarget = true;
    }

    private void PlaceNewTarget()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            tm.SpawnTarget(hitInfo.point);
        }
    }

    public void CamToRover()
    {
        mainCam.transform.position = tm.GetRoverPos() + new Vector3(0f, 2f, 0f);
    }

    public void ResetRotation()
    {
        mainCam.transform.rotation = Quaternion.identity;
    }

    public Vector3 GetCamEulers()
    {
        return mainCam.transform.localRotation.eulerAngles;
    }
}
