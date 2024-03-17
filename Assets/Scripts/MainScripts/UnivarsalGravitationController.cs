using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Linq;




public class UnivarsalGravitationController : MonoBehaviour
{
    [Tooltip("Please add the objects to be subjected to universal gravitation to the list by dragging and dropping them from the Hierarchy view.")]
    [SerializeField]
    public List<Rigidbody> TargetsList = new List<Rigidbody>();
    [Tooltip("全てのRigidbodyをリストに追加する場合はチェックをして下さい")]
    public bool AddAllRigidbody;

    // Coefficient to multiply for a given force (to visible easily)
    [SerializeField]
    [Range(1, 100000000000)] // Range is able to change if necessary
    public float COEFFICIENT;

    // Universal constant of gravitation
    [HideInInspector]
    public float CONSTANT;

    // Compute Shader
    [SerializeField]
    ComputeShader _computeShader;
    
    private ComputeBuffer _buffer_gravity;
    private ComputeBuffer _buffer_direction;

    void Awake()
    {
        // Add Rigidbodies to the list
        Rigidbody[] rblist = FindObjectsOfType<Rigidbody>();
            TargetsList = TargetsList.Union(rblist).ToList();

        // Calculate universal constant of gravitation
        CONSTANT = 6.674f * Mathf.Pow(10, -11);

        // Disable useGravity on all Rigidbodies
        for (int i = 0; i <= TargetsList.Count -1 ; i++){
            TargetsList[i].useGravity = false;
        }

        // Initialisation of compute buffer
        _buffer_gravity = new ComputeBuffer(1, sizeof(float)); 
        _buffer_direction = new ComputeBuffer(1, Marshal.SizeOf(typeof(Vector3))); 

        // Set buffers in compute shader 
        _computeShader.SetBuffer(_computeShader.FindKernel("UGCalc"), "ResultBuffer_gravity", _buffer_gravity);
        _computeShader.SetBuffer(_computeShader.FindKernel("UGCalc"), "ResultBuffer_direction", _buffer_direction);
    }


    void FixedUpdate()
    { 

        for (int i = 0; i <= TargetsList.Count -1 ;)
        {
            for(int n = 0; n <= TargetsList.Count -1 ;)
            {
                if (i != n)
                {
                    // Give the compute shader values and let them process
                    Vector3 i_position = TargetsList[i].transform.position;
                    Vector3 n_position = TargetsList[n].transform.position;

                    _computeShader.SetFloats("i_pos", new float[]{i_position.x, i_position.y, i_position.z});
                    _computeShader.SetFloats("n_pos",new float[]{n_position.x, n_position.y, n_position.z});
                    _computeShader.SetFloat("i_mass",TargetsList[i].mass);
                    _computeShader.SetFloat("n_mass",TargetsList[n].mass);
                    _computeShader.SetFloat("constant", CONSTANT);
                    
                    _computeShader.Dispatch(0, 1, 1, 1);
                    
                    // Get values
                    // An element of type float3 is converted into an individual element of float[3]
                    var data_gravity = new float[1];
                    var data_direction = new float[3];
                    _buffer_gravity.GetData(data_gravity);
                    _buffer_direction.GetData(data_direction);
                    

                    // Type conversion
                    float gravity = data_gravity[0];
                    Vector3 direction = new Vector3(data_direction[0],data_direction[1],data_direction[2]);
                    
                    // Add force
                    TargetsList[i].AddForce(gravity * direction * -1 * COEFFICIENT, ForceMode.Force);

                    /*
                    // Get the square of the distance between two points
                    Vector3 direction = TargetsList[i].transform.position - TargetsList[n].transform.position;
                    float distance = direction.magnitude;
                    distance *= distance;

                    // Calculate universal gravitation
                    float gravity = CONSTANT * TargetsList[i].mass * TargetsList[n].mass / distance;

                    // Add force
                    TargetsList[i].AddForce(gravity * direction.normalized * -1 * COEFFICIENT, ForceMode.Force);
                    */
                }
                n++;
            }
            i++;
        }
    }
    private void OnDestroy() 
    {
        _buffer_direction.Release();
        _buffer_gravity.Release();
    }
}


