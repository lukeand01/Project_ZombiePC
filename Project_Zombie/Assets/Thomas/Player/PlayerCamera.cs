

using DG.Tweening;
using MyBox;
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

        
        speed = 65;
    }

    private void Start()
    {
        originalRotation = cam.transform.localRotation;

        originalFOVValue = cam.fieldOfView;
        ActualFOVvalue = originalFOVValue;
        currentFOVValue = ActualFOVvalue;


        SetCamera(CameraPositionType.Default, 0, 0);
    }

    private void Update()
    {
        //DetectWalls();
    }

    private void FixedUpdate()
    {
        HandleFOVControl();
    }

    public void ResetPlayerCamera()
    {
        SetCamera(CameraPositionType.Default, 0f, 0f); //we instantly set the camera to the right position
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
        CallCameraRotation(Vector3.right, debugDistance, 1);
    }


    public void CallCameraRotation(Vector3 dir, float distanceStrenght, float modifier)
    {
        if(shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCameraProcess(dir, distanceStrenght, modifier));
    }

    public float GetValueForCameraShake(float x)
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
            return 1.2f - ((x - 2f) * (1.2f - 0.1f) / (25f - 2f)) ;
        }
    }

    IEnumerator ShakeCameraProcess(Vector3 dir, float distanceStrenght, float modifier)
    {

        //50 is the tolarable distance. meaning that at 50 the impact is 0.1
        //and at 2 or lower the impact is 2
        //50 

       
        float elapsed = 0.0f;
        float duration = 0.4f;
        float magnitude = GetValueForCameraShake(distanceStrenght) * modifier; 

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

    #region MOVE CAMERA TO ESPECIAL POSITIONS

    //and very importantly we need to rest the camera to the original position
    //default position, fall position, presentation position
    //we need to reset shake camera as well.
    //
    [SerializeField] CameraPositionClass[] cameraPositionClassArray;

    public void SetCamera(CameraPositionType _type,float timeToReachPosition, float timeToReachRotation)
    {
        //we set teh camera to the thing.     
        //we set it here.

        Transform refTransform = cameraPositionClassArray[(int)_type]._transform;


        cam.transform.DOKill();
        cam.transform.DOLocalMove(refTransform.localPosition, timeToReachPosition).SetEase(Ease.Linear);
        cam.transform.DOLocalRotate(refTransform.localRotation.eulerAngles, timeToReachRotation).SetEase(Ease.Linear);

        originalRotation = refTransform.localRotation;

    }


    

    //need to move the player position to be away from the camera. we get that position and set the player position a bit behind.

    //so for now at the start we call it and also at reset.
    

    #endregion

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;


        //Gizmos.DrawLine(_cam.transform.position, transform.position - _cam.transform.position);
    }
}

public enum CameraPositionType
{
    Default = 0,
    FallDeath = 1,
    Presentation = 2,
    RegularDeath = 3,
    Dialogue = 4
}
[System.Serializable]
public class CameraPositionClass
{
    [field: SerializeField] public CameraPositionType _type { get; private set; }
    [field: SerializeField] public Transform _transform { get; private set; }

}
