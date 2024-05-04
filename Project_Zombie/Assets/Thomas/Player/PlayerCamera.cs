

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    LayerMask wallLayer;


    Wall currentWall;

    Camera cam;

    private void Awake()
    {
        wallLayer |= (1 << 9);

        cam = Camera.main;
    }

    private void Update()
    {
        DetectWalls();
    }


    void DetectWalls()
    {

        Vector3 direction = transform.position - cam.transform.position;


        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, direction, out hit, Mathf.Infinity))
        {
            if(hit.collider != null) 
            { 
                if(hit.collider.gameObject.layer == 9)
                {
                    Wall wall = hit.collider.GetComponent<Wall>();

                    if(wall == null)
                    {
                        Debug.Log("found it but there was no wall there");
                        return;
                    }

                    if(currentWall != null)
                    {
                        if (currentWall.IsSame(wall.id))
                        {
                            return;
                        }
                        currentWall.ChangeMaterial(false);
                    }

                    currentWall = wall;
                    currentWall.ChangeMaterial(true);


                }
                else
                {
                    if(currentWall != null)
                    {
                        currentWall.ChangeMaterial(false);
                        currentWall = null;
                    }
                }
            
            
            }

        }
        
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;


        //Gizmos.DrawLine(cam.transform.position, transform.position - cam.transform.position);
    }
}
