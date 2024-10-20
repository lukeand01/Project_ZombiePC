
using UnityEngine;

public class FireFlyingBlade : MonoBehaviour
{

    [SerializeField] Vector3 _dir;
    DamageClass _damage;
    float _speed;
    bool _isMoving = false;

    public void SetUp(Vector3 dir, DamageClass damage, float speed)
    {
        _dir = dir;
        _damage = damage;
        _speed = speed;
    }


    private void Update()
    {
        //keep moving on the direction.
        //i want it to keep moving to the side.


        if (_isMoving) return;

        transform.position += _dir * _speed * Time.deltaTime;
        _dir += new Vector3(0.1f, 0.1f) * Time.deltaTime * 0.01f;
        transform.Rotate(new Vector3(0, 300 * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        //if enters a wall it bounces in the opposite direction

        if(other.gameObject.layer == 9)
        {
            //Vector3 oppositeDir = (other.transform.position - transform.position).normalized;
            //_dir = oppositeDir;
            _dir = GetRandomOppositeDirection(other.transform);
            _dir.y = 0;
        }

        if(other.gameObject.layer == 3)
        {
            //deal damage
            PlayerHandler.instance._playerResources.TakeDamage(_damage);
        }


    }

    private Vector3 GetRandomOppositeDirection(Transform collidedObject)
    {
        // Calculate the direction from the player to the last touched object
        Vector3 directionToLastObject = transform.position - collidedObject.position;

        // Get the opposite direction
        Vector3 oppositeDirection = -directionToLastObject.normalized;

        // Optionally, add a random offset for some variation
        float randomAngle = Random.Range(-30f, 30f); // Adjust this range for more randomness
        oppositeDirection = Quaternion.Euler(0, randomAngle, 0) * oppositeDirection;

        return oppositeDirection.normalized;
    }

}
