using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("Noir Project/Tools/Mesh Lenght Finder")]
[ExecuteInEditMode]
public class MeshLenghtFinder : MonoBehaviour {

    [HideInInspector]
    public float meshLenghtResult = 0.0f;
    public float meshZPosition = 0.0f;

    public enum DistanceAxis
    {
        X,
        Y,
        Z
    }

    public DistanceAxis distanceAxis = DistanceAxis.X;

    public enum DisplayStyle
    {
        Wire,
        Solid
    }

    public DisplayStyle displayStyle = DisplayStyle.Wire;

    [Range (0.01f, 20.0f)]
    public float distance = 1.0f;

    [HideInInspector]
    public float othersidesDistance = 1.0f;

    //this will return current mesh lenght
    public float getMeshLenght()
    {
        return meshLenghtResult;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;

        if (distanceAxis == DistanceAxis.X)
        {
            if (displayStyle == DisplayStyle.Wire)
                Gizmos.DrawWireCube(transform.position, new Vector3(distance, othersidesDistance, othersidesDistance));
            if (displayStyle == DisplayStyle.Solid)
                Gizmos.DrawCube(transform.position, new Vector3(distance, othersidesDistance, othersidesDistance));        
        }

        if (distanceAxis == DistanceAxis.Y)
        {
            if (displayStyle == DisplayStyle.Wire)
                Gizmos.DrawWireCube(transform.position, new Vector3(othersidesDistance, distance, othersidesDistance));
            if (displayStyle == DisplayStyle.Solid)
                Gizmos.DrawCube(transform.position, new Vector3(othersidesDistance, distance, othersidesDistance)); 
        }

        if (distanceAxis == DistanceAxis.Z)
        {
            if (displayStyle == DisplayStyle.Wire)
                Gizmos.DrawWireCube(transform.position, new Vector3(othersidesDistance, othersidesDistance, distance));
            if (displayStyle == DisplayStyle.Solid)
                Gizmos.DrawCube(transform.position, new Vector3(othersidesDistance, othersidesDistance, distance)); 
        }

        meshLenghtResult = distance;
        meshZPosition = (distance / 2) / transform.localScale.z;
        meshZPosition *= -1;

    }
}
