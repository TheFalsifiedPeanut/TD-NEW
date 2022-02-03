using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

struct PathData
{
    public List<Waypoint> waypoints;
    

    public PathData(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
    }
}

[Serializable]
struct WaypointData
{
    public TargetingCondition targetingcondition;
    public Waypoint targetWaypoint;
}

public class Waypoint : MonoBehaviour
{
    [SerializeField] WaypointData[] possibleWaypoints;
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if(possibleWaypoints != null)
        {
            if (other.GetComponent<Unit>())
            {
                PathData pathData = PathSearch(0, other.GetComponent<Unit>(), this, new List<PathData>(), null);
                if (pathData.target != null)
                {
                    
                    other.GetComponent<Unit>().SetMovementTarget(pathData.target.gameObject);
                }
            }
        }
    }*/
    
    /*PathData TreeSearch(int shortestLength, Unit unit, Waypoint currentWaypoint)
    {
        //Tracks the ID of closest waypoint.
        int shortestChild = int.MaxValue;
        //References the closest waypoint.
        Waypoint shortestWaypoint = null;
        //Increase the shortest path Length by 1.
        shortestLength++;
        //Loop for the possible waypoints for the current  waypoint.
        for (int i = 0; i < currentWaypoint.possibleWaypoints.Length; i++)
        {
            Debug.Log("Unit Name:" + unit.gameObject.name +
            "\n Target Condition: " + currentWaypoint.possibleWaypoints[i].targetingcondition +
            "\n Unit Target: " + unit.GetTargetingCondition()); 
            //Checks if the current possible waypoint has a matching targeting condition as the unit.
            if (currentWaypoint.possibleWaypoints[i].targetingcondition == unit.GetTargetingCondition())
            {
                Debug.Log(currentWaypoint.possibleWaypoints[i].targetWaypoint);
                //Return pathdata including the path length and the target waypoint gameobject.
                return new PathData(shortestLength, currentWaypoint.possibleWaypoints[i].targetWaypoint);
            }
            //If not, we check if the shortest path is still less that 5.
            else if(shortestLength < 5)
            {
                //Cache pathdata from the recursive treesearch.
                PathData pathData = TreeSearch(shortestLength, unit, currentWaypoint.possibleWaypoints[i].targetWaypoint);
                //Check ig the length of the path is less than the current shortest path.
                if (pathData.length < shortestChild)
                {
                    //replace the old shortest path with the shorter.
                    shortestChild = pathData.length;
                    //Set the shortest waypoint to be the targetwaypoint.
                    shortestWaypoint = currentWaypoint.possibleWaypoints[i].targetWaypoint;
                }
            }
        }
        Debug.Log(shortestWaypoint);
        return new PathData(shortestChild, shortestWaypoint);
        
    }*/

    PathData PathSearch(int shortestLength, Unit unit, Waypoint currentWapoint, List<PathData> pathData, Waypoint waypointData)
    {
        shortestLength++;
        if (shortestLength <= 7) 
        {
            for (int i = 0; i < currentWapoint.possibleWaypoints.Length; i++)
            {
                waypointData = waypointData == null ? currentWapoint.possibleWaypoints[i].targetWaypoint : waypointData;
                if (currentWapoint.possibleWaypoints[i].targetingcondition == unit.GetTargetingCondition())
                {
                    pathData.Add(new PathData(shortestLength, waypointData));
                }
                else
                {
                    pathData.Add(PathSearch(shortestLength, unit, currentWapoint.possibleWaypoints[i].targetWaypoint, pathData, waypointData)) ; 
                }
            }
        }
        pathData.OrderBy((x) => x.length);
        PathData[] shorterPaths = pathData.Where((x) => x.length <= shortestLength).ToArray();
        if (shorterPaths.Length > 0)
        {
            return shorterPaths[UnityEngine.Random.Range(0, shorterPaths.Length)];
        }
        return new PathData();
    }

    public PathData TrackSearch(Unit unit, Waypoint waypoint, PathData pathData)
    {
        List<PathData> pathDatas = new List<PathData>();
        for (int i = 0; i < waypoint.possibleWaypoints.Length; i++)
        {
            WaypointData waypointData = waypoint.possibleWaypoints[i];
            if (waypointData.targetingcondition == unit.GetTargetingCondition())
            {
                PathData currentPath = new PathData(pathData.waypoints);
                currentPath.waypoints.Add(waypointData.targetWaypoint);
            }
        }
    }
}




