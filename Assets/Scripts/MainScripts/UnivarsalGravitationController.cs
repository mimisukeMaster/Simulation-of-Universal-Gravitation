using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;



public class UnivarsalGravitationController : MonoBehaviour
{
    [Tooltip("Please add the objects to be subjected to universal gravitation to the list by dragging and dropping them from the Hierarchy view.")]
    [SerializeField]
    public List<Rigidbody> TargetsList = new List<Rigidbody>();
    [Tooltip("Turn on to add all Rigidbodies to the list.")]
    public bool AddAllRigidbody;

    // Coefficient to multiply for a given force (to visible easily)
    [SerializeField]
    [Range(1, 100000000000)] // Range is able to change if necessary
    public float COEFFICIENT;

    // Universal constant of gravitation
    [HideInInspector]
    public float CONSTANT;


    [SerializeField]
    private ComputeShader _computeShader;

    private ComputeBuffer _input_buffer;

    private ComputeBuffer _result_buffer;


    private struct InputBufferData
    {
        public float mass;
        public Vector3 position;
    }

    private struct ResultBufferData
    {  
        public Vector3 force; 
    }

    /// <summary>
    /// List of data to be sent to ComputeShader
    /// </summary>
    private List<InputBufferData> inputBufferDataList  = new List<InputBufferData>();

    /// <summary>
    /// List to ensure capacity for the data received
    /// </summary>
    private List<ResultBufferData> initialBufferDataList = new List<ResultBufferData>();

    /// <summary>
    /// Array to store the data received
    /// </summary>
    private ResultBufferData[] resultBufferDataArray;

    private int _kernel;


    void Awake()
    {
        // Add Rigidbodies to the Target List
        if(AddAllRigidbody){
        Rigidbody[] rblist = FindObjectsOfType<Rigidbody>();
        TargetsList = TargetsList.Union(rblist).ToList();
        }
        else if(TargetsList.Count == 0){
            Debug.LogError("Please add Rigidbodies to the Target List.");
            EditorApplication.isPlaying = false;
        }

        // Calculate universal constant of gravitation
        CONSTANT = 6.674f * Mathf.Pow(10, -11);

        // Disable useGravity on all Rigidbodies
        for (int i = 0; i < TargetsList.Count; i++) TargetsList[i].useGravity = false;

        // Initialisation of compute buffer
        _input_buffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(InputBufferData)));
        _result_buffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(ResultBufferData)));

        // Set buffers in compute shader 
        _kernel = _computeShader.FindKernel("UGCalc");
        _computeShader.SetBuffer(_kernel, "InputBuffer", _input_buffer);
        _computeShader.SetBuffer(_kernel, "ResultBuffer", _result_buffer);
        
        // Set each value
        _computeShader.SetFloat("constant", CONSTANT);
        _computeShader.SetFloat("coefficient",COEFFICIENT);
        _computeShader.SetInt("list_count", TargetsList.Count);

        // Initialized by setting the number of elements in the array to be received
        resultBufferDataArray = new ResultBufferData[TargetsList.Count];
    }


    void FixedUpdate()
    {
        for (int i = 0; i < TargetsList.Count; i++)
        {
            // Create InputBufferData one by one from TargetList and add them to the list
            InputBufferData inputBufferData = new InputBufferData(){
                mass = TargetsList[i].mass,
                position = TargetsList[i].transform.position
            };
            inputBufferDataList.Add(inputBufferData);
        }
        
        // Send the list via Buffer to the ComputeShader 
        // Takes the form of an argument that can specify the list to be set and
        // the number of elements in it

        //_result_bufferには前回の時のGetしたやつが残っているから、Setして一掃する！要素数は同じものを指定している
        _input_buffer.SetData(inputBufferDataList, 0, 0, inputBufferDataList.Count);
        //_result_buffer.SetData(initialBufferDataList, 0, 0, initialBufferDataList.Count);


        _computeShader.Dispatch(_kernel,inputBufferDataList.Count,1,1);

        // receive data array
        _result_buffer.GetData(resultBufferDataArray);

        // Empty the elements of the used list
        for (int i = 0; i < resultBufferDataArray.Length; i++){ 
            TargetsList[i].AddForce(resultBufferDataArray[i].force, ForceMode.Force);
        }

        inputBufferDataList.Clear();
    }

    private void OnDestroy() 
    {
        _input_buffer.Release();
        _result_buffer.Release();
    }
}


