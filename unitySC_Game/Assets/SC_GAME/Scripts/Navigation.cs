using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    struct navPoint
    {
        public int             id;
        public Vector3         location;
        public List<int>       joints;
        public List<float>     distances;
    }
    
    public Material debugMaterial;


    private List<navPoint>  navPoints;
    private GameObject      NavParent;

    //For Agents to Navigate
    public Vector3         PlayerLocation = Vector3.zero;
    public Vector3         PlayerPoint  = Vector3.zero;
    


    public void CreateNavMap()
    {
        navPoints = new List<navPoint>();
        NavParent = GameObject.Find("_PATHS");


        for (int i = 0; i < NavParent.transform.childCount; i++)
        {
            Transform transPoint = NavParent.transform.GetChild(i);
            navPoint newPoint;
            newPoint.id = i;
            newPoint.location = transPoint.position;
            newPoint.joints = new List<int>();
            newPoint.distances = new List<float>();

            //now lets add some joints into it
            for(int j = 0; j < NavParent.transform.childCount; j++)
            {
                if (i == j)
                    continue;

                RaycastHit ray;
                Transform target = NavParent.transform.GetChild(j);

                if (!Physics.Linecast(newPoint.location, target.position, out ray))
                {   
                    newPoint.joints.Add(j);
                    newPoint.distances.Add(Vector3.Distance(newPoint.location, target.position));

                    GameObject lineObject = new GameObject("navLine" + i * j);//ONLY ONE LRENDERER PER OBJECT, WHAT A SCAM!!!!
                    LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;

                    lineRenderer.SetPosition(0, newPoint.location);
                    lineRenderer.SetPosition(1, target.position);

                    lineRenderer.startWidth = 0.015f;
                    lineRenderer.endWidth = 0.015f;

                    lineRenderer.material = debugMaterial;
                    lineRenderer.material.color = Color.yellow;

                    lineObject.transform.SetParent(NavParent.transform.parent);
                }   
            }
            navPoints.Add(newPoint);
        }
    }



    public Vector3 getClosestPoint(Vector3 position)
    {
        float closestDistance    = float.MaxValue;
        int index                = -1;

        foreach (navPoint point in navPoints)
        {
            float distance = Vector3.Distance(point.location, position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                index = point.id;
            }
        }

        if (index != -1)
            return navPoints[index].location;
        else
            return position; 
    }



    void updateLocation(Vector3 position)
    {
        PlayerLocation = position;
        PlayerPoint = getClosestPoint(position);

        NavParent.GetComponent<LineRenderer>().SetPositions(new Vector3[] {PlayerLocation, PlayerPoint});
    }



    ////////////////////////////
    ////Events
    private void OnEnable()
    {
        // Subscribe to the OnSomethingFound event
        controller_player.PlayerLocation += updateLocation;
    }
    private void OnDisable()
    {
        // Unsubscribe from the OnSomethingFound event
        controller_player.PlayerLocation -= updateLocation;
    }
    //////////-Events
    /////////////////////////
}





