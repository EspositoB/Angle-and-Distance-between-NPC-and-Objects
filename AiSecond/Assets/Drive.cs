using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float moveSpeed = 10f;
    public float rSpeed = 0.01f;
    
    public GameObject fuel;



    Vector3 movementDirection;

    void Start()
    {

    }

    void CalculateDistance()
    {
       float distance = Mathf.Sqrt(Mathf.Pow(fuel.transform.position.x - this.transform.position.x,2) 
           + Mathf.Pow(fuel.transform.position.z - this.transform.position.z,2));

        Vector3 fuelPos = new Vector3(fuel.transform.position.x, 0, fuel.transform.position.z);
        Vector3 tankPos = new Vector3(transform.position.x, 0, transform.position.z);

        float uDistance = Vector3.Distance(fuelPos, tankPos);
        Vector3 tankToFuel = fuelPos - tankPos;

        Debug.Log("Distance is: " + distance);
        Debug.Log("Distance is: " + uDistance);
        Debug.Log("Distance is: " + tankToFuel.magnitude);
        Debug.Log("Distance is: " + tankToFuel.sqrMagnitude);
    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {

        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.x * w.z - v.z * w.x;
        float zMult = v.x * w.y - v.y * w.x;

        return new Vector3(xMult, yMult, zMult);
    }

    void CalculateAngle()
    {
        Vector3 forwardTank = transform.up;
        Vector3 fuelDirection = fuel.transform.position - transform.position;

        Debug.DrawRay(transform.position, forwardTank, Color.green, 2);
        Debug.DrawRay(transform.position, fuelDirection, Color.red, 2);

        float dot = forwardTank.x * fuelDirection.x + forwardTank.y * fuelDirection.y;
        float angle = Mathf.Acos(dot / (forwardTank.magnitude * fuelDirection.magnitude));

        Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        Debug.Log("Unity Angle: " + Vector3.Angle(forwardTank, fuelDirection));

        int clockWise = 1;
        
        if (Cross(forwardTank, fuelDirection).z < 0)
        {
            clockWise = -1;
        }

        this.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockWise * rSpeed);
    }

    void MovementWithT()
    {
        movementDirection = fuel.transform.position - transform.position;
        if (movementDirection.magnitude >1)
        {
            Vector3 velocity = movementDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += velocity;
            Debug.Log("Arrived!!");
        }
    }

    void LateUpdate()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, -rotation);

        if (Input.GetKey(KeyCode.Space))
        {
            CalculateDistance();
            CalculateAngle();
        }

        if (Input.GetKey(KeyCode.T))
        {
            MovementWithT();  
        }
    }
}