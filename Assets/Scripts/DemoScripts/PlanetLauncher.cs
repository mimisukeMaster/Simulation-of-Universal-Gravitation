using UnityEngine;

public class PlanetLauncher : MonoBehaviour
{        
    [Tooltip("Specify the fixed star(ex: sun)")]
    public GameObject FixedStar;
        
    [Space(25)]
    public UniversalGravitationController UGController;
    
    
    void Start()
    {
        Vector3 myVec = transform.position;
        Vector3 CentralVec = FixedStar.transform.position;
        
        // Calculate launch velocity
        var G = UGController.CONSTANT * UGController.COEFFICIENT;
        var M = FixedStar.GetComponent<Rigidbody>().mass;
        var r = Vector3.Distance(myVec, CentralVec);

        // Launch in a direction normal to the vector between the two points
        float initSpeed = (float)System.Math.Sqrt(G * M / r);
        
        Vector3 initVelocity = 
            Quaternion.Euler(0, 90, 0) * (CentralVec - myVec).normalized * initSpeed;

        GetComponent<Rigidbody>().velocity = initVelocity;
    }
}
