

using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{



    PlayerHandler handler;
    [SerializeField] Camera cam;


    private void Awake()
    {
        wallLayer |= (1 << 9);

        handler = GetComponent<PlayerHandler>();

        originalRotation = cam.transform.localRotation;

        originalFOVValue = cam.fieldOfView;
        ActualFOVvalue = originalFOVValue;
        currentFOVValue = ActualFOVvalue;
        speed = 65;
    }

    private void Update()
    {
        DetectWalls();
    }

    private void FixedUpdate()
    {
        HandleFOVControl();
    }

    #region DETECT WALLS
    LayerMask wallLayer;
    Wall currentWall;
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

    #endregion

    #region SHAKE CAMERA 
    //
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;
    public AnimationCurve shakeCameraAnimationCurve;

    [SerializeField] float debugDistance;

    [ContextMenu("CALL CAMERA ROTATION")]
    public void DebugCallRotation()
    {
        CallCameraRotation(Vector3.right, debugDistance);
    }


    public void CallCameraRotation(Vector3 dir, float distanceStrenght)
    {
        if(shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCameraProcess(dir, distanceStrenght));
    }

    public float GetValue(float x)
    {
        if (x <= 2)
        {
            return 1.2f;
        }
        else if (x >= 25)
        {
            return 0.1f;
        }
        else
        {
            // Linear interpolation between 2 and 0.1
            return 1.2f - ((x - 2f) * (1.2f - 0.1f) / (25f - 2f));
        }
    }

    IEnumerator ShakeCameraProcess(Vector3 dir, float distanceStrenght)
    {

        //50 is the tolarable distance. meaning that at 50 the impact is 0.1
        //and at 2 or lower the impact is 2
        //50 

       
        float elapsed = 0.0f;
        float duration = 0.4f;
        float magnitude = GetValue(distanceStrenght);

        Vector3 dirWeight = dir * 3;

        float offsetValue = 0.5f;

        while (elapsed < duration)
        {
            float strength = shakeCameraAnimationCurve.Evaluate(elapsed / duration) * magnitude;
            float x = Random.Range(-offsetValue, offsetValue) * strength * dirWeight.y;
            float y = Random.Range(-offsetValue, offsetValue) * strength * dirWeight.x;
            float z = Random.Range(-1f, 1f) * strength;


            cam.transform.localRotation = originalRotation * Quaternion.Euler(x, y, z);

            elapsed += Time.fixedDeltaTime;

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }

        cam.transform.localRotation = originalRotation;
    }

    #endregion

    #region ZOOM 
    //

    float originalFOVValue;
    float ActualFOVvalue;
    float currentFOVValue;
    float speed;

    public void ControlFieldOfView(float newValue)
    {
        ActualFOVvalue = newValue;  
    }
    public void ReturnFieldOfViewToOriginal()
    {
        ActualFOVvalue = originalFOVValue;
    }

    void HandleFOVControl()
    {
        if(currentFOVValue > ActualFOVvalue)
        {
            //then this means we have to reduce it.
            currentFOVValue -= Time.deltaTime * speed;
            cam.fieldOfView = currentFOVValue;
        }
        if (currentFOVValue < ActualFOVvalue)
        {
            currentFOVValue += Time.deltaTime * speed;
            cam.fieldOfView = currentFOVValue;
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;


        //Gizmos.DrawLine(cam.transform.position, transform.position - cam.transform.position);
    }
}
