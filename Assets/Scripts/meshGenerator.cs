using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class meshGenerator : MonoBehaviour {

    [SerializeField] float angleInRad = 0.5f;
    [SerializeField] float length = 1f;
    
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateTriangleMesh(length);
        Player.Instance.OnChargeUpdate += MeshGenerator_OnMeshUpdate;
    }

    private void MeshGenerator_OnMeshUpdate(object sender, Player.OnChargeUpdateEventArgs e)
    {
        mesh.Clear();
        CreateTriangleMesh(e.charge + length);
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void CreateTriangleMesh(float length) {
        float width = Mathf.Tan(angleInRad) * length;
        vertices = new Vector3[] {new Vector3(0,0,0), new Vector3(-width,0,length), new Vector3(width,0,length)};
        triangles = new int[] {0, 1, 2};
    }

    public float GetAngleInRad() {
        return angleInRad;
    }
}
