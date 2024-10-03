using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLineOfSight : MonoBehaviour
{
    //this is responsble for creating line of sight
    [Separator("COMPONENT")]
    [SerializeField] MeshFilter _meshFilter;
    Mesh _mesh;
    LayerMask layersThatStopVision;
    PlayerHandler handler;


    //we need to fix this thing being stuck
    //

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();    
    }

    private void Start()
    {
        if (_meshFilter == null) return;
        layersThatStopVision |= (1 << 9);
        //layersThatStopVision |= (1 << 7);
        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

      

    }

    private void Update()
    {
       HandleMesh();

    }

    [Separator("VARIABLES")]
    [SerializeField] float fov = 90;
    [SerializeField] int rayCount = 2;   
    [SerializeField] float viewDistance = 50;


    [SerializeField] Vector3 originDebug;

    [SerializeField] float startingAngle;
    //the problem is that the calculation isnt coming exactly from the player. the numbers are a bit off
    //

    public void HandleMesh()
    {

        if (_mesh == null) return;
        SetStartingAngle();

        float angle = 0;
        Vector3 origin = transform.position;
        float angleIncrease = fov / rayCount;
        

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];


        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 direction = MyUtils.GetVectorFromAngle(angle);
            Vector3 vertex = origin;

            RaycastHit hit;

           bool hasHit = Physics.Raycast(origin, direction, out hit, viewDistance, layersThatStopVision);

            if (hasHit)
            {
                vertex = hit.point; 
            }
            else
            {
                vertex = origin + direction * viewDistance;
            }

            vertices[vertexIndex] = vertex;

            if(i > 0)
            {
                triangles[triangleIndex + 0] = 0;

                triangles[triangleIndex + 1] = vertexIndex - 1;

                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++; 


            angle -= angleIncrease;
        }

        _mesh.vertices = vertices;
        _mesh.uv = uv;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

    }


    void SetStartingAngle()
    {
        Vector3 mousePos = handler._cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 MousePosInRelationToPlayer = mousePos - transform.position;
        startingAngle = MyUtils.GetAngleFromVectorFloat(MousePosInRelationToPlayer) - fov / 2;
    }
}
