using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;

public class ARController : MonoBehaviour
{




    private List<DetectedPlane> m_NewTrackedPlanes = new List<DetectedPlane>();
    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check ARCore sessions status
        if(Session.Status != SessionStatus.Tracking)
        {

            return;
        }


        Session.GetTrackables<DetectedPlane>(m_NewTrackedPlanes, TrackableQueryFilter.New);

        for(int i =0; i< m_NewTrackedPlanes.Count; ++i)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);


            grid.GetComponent<GridVisualizer>().Initialize(m_NewTrackedPlanes[i]);

        }


        //check if any input
        Touch touch;
        if (Input.touchCount <1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;

        }

        //if user touches any tracked planes
        TrackableHit hit;
        if(Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon,out hit))
        {

            //place the portal

            //enable the portal
            Portal.SetActive(true);

            //create an anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            // place facing camera

            Vector3 cameraPosition = ARCamera.transform.position;

            cameraPosition.y = hit.Pose.position.y;

            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            Portal.transform.parent = anchor.transform;

        }




    }
}
