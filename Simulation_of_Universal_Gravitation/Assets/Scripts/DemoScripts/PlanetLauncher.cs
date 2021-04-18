using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLauncher : MonoBehaviour
{
    /// <summary>
    /// 単位については全てScene上のそのままの値を使用
    /// G：万有引力定数(N・m^2/kg^2 )
    /// M：太陽の質量(kg)
    /// r：太陽と自惑星との距離
    /// </summary>
    
    public float TorqueSpeed = 5; 
        
    [Header("Specify the central celestial body")]
    public GameObject Central_Celestialbody;
        
    [Space(25)]
    public GameObject UG_Director;
    // Start is called before the first frame update
    void Start()
    {   

    }
    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount == 1){      //Startに書くとvar G が0を取得してしまう為

        Vector3 myVec = this.transform.position;
        Vector3 CentralVec = this.Central_Celestialbody.transform.position;
        
        //LaunchSpeed Calculation
        var G = this.UG_Director.GetComponent<UnivarsalGravitationController>().coefficient;//万有引力を引用
        var M = this.Central_Celestialbody.GetComponent<Rigidbody>().mass;//中心となる天体(太陽)の質量
        var r = Vector3.Distance(myVec, CentralVec);//２点間の距離
        
        float initVelocityZ; //初速度m/s
        
        initVelocityZ = (float)System.Math.Sqrt(G * M / r);
        
        Vector3 initVelocity  = new Vector3(0f, 0f, initVelocityZ);
        GetComponent<Rigidbody>().velocity = initVelocity;

        
        }
        transform.Rotate(new Vector3(0, -TorqueSpeed,0));//自転を再現
        
        
    }
}
