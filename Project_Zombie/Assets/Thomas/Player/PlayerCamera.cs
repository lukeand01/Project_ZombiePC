

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    LayerMask wallLayer;

    PlayerHandler handler;
    Wall currentWall;


    private void Awake()
    {
        wallLayer |= (1 << 9);

        handler = GetComponent<PlayerHandler>();    
    }

    private void Update()
    {
        DetectWalls();
    }


    void DetectWalls()
    {
        if (handler._cam == null) return;

        Vector3 direction = transform.position - handler._cam.transform.position;


        RaycastHit hit;

        if (Physics.Raycast(handler._cam.transform.position, direction, out hit, Mathf.Infinity))
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
