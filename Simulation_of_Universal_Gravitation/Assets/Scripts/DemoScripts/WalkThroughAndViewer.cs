using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkThroughAndViewer : MonoBehaviour
{
    [SerializeField]
    Transform CenterOfBalance;  // 重心となる子Object
    Camera mainCamera;
    //public float speed = -3.40282347E+38F;
     public Vector2 rotationSpeed = new Vector2(0.1f, 0.2f);
    private Vector2 lastMousePosition;
    private Vector2 newAngle = new Vector2(0, 0);
    
    bool FirstLook = true;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            newAngle = mainCamera.transform.eulerAngles;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            FirstLook = false;

            newAngle.y -= (lastMousePosition.x - Input.mousePosition.x) * rotationSpeed.y;
            newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * rotationSpeed.x;
            mainCamera.transform.eulerAngles = newAngle;
            lastMousePosition = Input.mousePosition;
            
            //SaveVec += new Vector3(newAngle.x,newAngle.y,0);
        }else{
            if(FirstLook){
                gameObject.transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x,
                    transform.rotation.y + 90,
                    transform.rotation.z)
                );
            }if(!FirstLook){
                gameObject.transform.rotation = Quaternion.Euler(
                new Vector3(transform.rotation.x + newAngle.x,
                    transform.rotation.y + newAngle.y,
                    transform.rotation.z)
                );
            }
            

        }

        
        // キーボード入力で移動
        float dy = Input.GetAxis ("Vertical");
        transform.Translate (0.0f,dy*0.01f,0.0f );



        /*
        RaycastHit hit;

        // Transformの真下の地形の法線を調べるcenterからmonoe
        if(Physics.Raycast(CenterOfBalance.position, -transform.up,out hit,
                    float.PositiveInfinity)){
            
            


            // 傾きの差を求める
            Quaternion q = Quaternion.LookRotation(
                    transform.localPosition.normalized,
                    hit.normal);

            // 自分を回転させる
            transform.rotation *= q;
            Debug.Log("RaycastIF + " + transform.localPosition);
            
            // 地面から一定距離離れていたら落下
            if (hit.distance > 0.5f) {
                Debug.Log("distance > 0.5f");
                Vector3 localVecTr = transform.InverseTransformPoint(transform.position);
                localVecTr = new Vector3(0,-1,0);
                
                transform.position =
                    transform.position +
                    (localVecTr /** Physics.gravity.magnitude* Time.fixedDeltaTime);
            }
        }
        */
    }
}
