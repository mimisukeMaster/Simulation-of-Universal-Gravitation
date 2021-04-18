using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometLauncher : MonoBehaviour
{

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
        
        //First-cosmic-velocity Calculation
        var G = this.UG_Director.GetComponent<UnivarsalGravitationController>().coefficient;//万有引力を引用
        var M = this.Central_Celestialbody.GetComponent<Rigidbody>().mass;//中心となる天体(太陽)の質量
        var r = Vector3.Distance(myVec, CentralVec);//２点間の距離
        
        float FirstVelocity; //第一宇宙速度m/s
        
        FirstVelocity = (float)System.Math.Sqrt(G * M / r);//第一宇宙速度設定


        //\\
        //Second-cosmic-velocity Calculation
        //Second-cosmic-velocityはFirst-cosmic-velocityのちょうど√2倍であるので
        //そのまま√2倍の値を設定
        float SecondVelocity;

        SecondVelocity = FirstVelocity * (float)System.Math.Sqrt(2);

        float ThroughVelocity = SecondVelocity - (SecondVelocity - FirstVelocity) / 5 ;
        GetComponent<Rigidbody>().velocity = transform.right*ThroughVelocity;//new Vector3(ThroughVelocity, 0, 0);


        
        }

    }
}
