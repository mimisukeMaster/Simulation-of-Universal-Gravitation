using UnityEngine;

public class CometLauncher : MonoBehaviour
{
        
    [Header("Specify the fixed star (ex: sun)")]
    public GameObject FixedStar;
    [Range(1, 1.414f)]
    public float COMET_COEFFICIENT;
        
    [Space(25)]
    public UniversalGravitationController UGController;
    
    
    void Start()
    {
        Vector3 myVec = transform.position;
        Vector3 OriginVec = FixedStar.transform.position;
        
        // Calculate launch velocity
        var G = UGController.CONSTANT * UGController.COEFFICIENT;
        var M = FixedStar.GetComponent<Rigidbody>().mass;
        var r = Vector3.Distance(myVec, OriginVec);

        // Launch in a direction normal to the vector between the two points
        float initSpeed = (float)System.Math.Sqrt(G * M / r);
        
        Vector3 initVelocity = 
            Quaternion.Euler(0, 90, 0) * (OriginVec - myVec).normalized * initSpeed * COMET_COEFFICIENT;

        GetComponent<Rigidbody>().velocity = initVelocity;
    }
}
