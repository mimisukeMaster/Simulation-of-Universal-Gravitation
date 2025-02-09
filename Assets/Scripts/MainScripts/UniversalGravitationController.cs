using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;



public class UniversalGravitationController : MonoBehaviour
{
    [Tooltip("Please drag the Rigidbodies you want to apply universal gravitation to the list from the Hierarchy view.")]
    [SerializeField]
    public List<Rigidbody> TargetsList;
    [Tooltip("Enable this to add all Rigidbodies to the list.")]
    public bool AddAllRigidbody;

    // Coefficient to multiply for a given force to make it more visible
    // The range can be changed if necessary
    [SerializeField]
    [Range(1, 100000000000)]
    public float COEFFICIENT;

    // Gravitational constant
    [HideInInspector]
    public float CONSTANT;


    [SerializeField]
    private ComputeShader computeShader;

    private ComputeBuffer inputBuffer;

    private ComputeBuffer resultBuffer;

    private int kernel;


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
    /// List of data to be sent to compute shader
    /// </summary>
    private List<InputBufferData> inputBufferDataList  = new List<InputBufferData>();

    /// <summary>
    /// Array to store the data received
    /// </summary>
    private ResultBufferData[] resultBufferDataArray;


    void Awake()
    {
        // Add Rigidbodies to the Target List
        if (AddAllRigidbody)
        {
            Rigidbody[] rblist = FindObjectsOfType<Rigidbody>();
            TargetsList = TargetsList.Union(rblist).ToList();
        }
        else if (TargetsList.Count == 0)
        {
            Debug.LogError("Please add Rigidbodies to the Target List.");
            EditorApplication.isPlaying = false;
        }

        // Calculate gravitational constant
        CONSTANT = 6.674f * Mathf.Pow(10, -11);

        // Disable useGravity on all Rigidbodies
        for (int i = 0; i < TargetsList.Count; i++) TargetsList[i].useGravity = false;

        // Initialization of compute buffer
        inputBuffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(InputBufferData)));
        resultBuffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(ResultBufferData)));

        // Set buffers in compute shader 
        kernel = computeShader.FindKernel("UGCalculator");
        computeShader.SetBuffer(kernel, "InputBuffer", inputBuffer);
        computeShader.SetBuffer(kernel, "ResultBuffer", resultBuffer);
        
        // Set each value
        computeShader.SetFloat("constant", CONSTANT);
        computeShader.SetFloat("coefficient",COEFFICIENT);
        computeShader.SetInt("listCount", TargetsList.Count);

        // Initialize by defining the number of elements the array will receive
        resultBufferDataArray = new ResultBufferData[TargetsList.Count];
    }


    void FixedUpdate()
    {
        for (int i = 0; i < TargetsList.Count; i++)
        {
            // Create InputBufferData one by one from TargetList and add them to the list
            InputBufferData inputBufferData = new InputBufferData()
            {
                mass = TargetsList[i].mass,
                position = TargetsList[i].transform.position
            };
            inputBufferDataList.Add(inputBufferData);
        }
        
        // Send the list via Buffer to the compute shader 
        inputBuffer.SetData(inputBufferDataList, 0, 0, inputBufferDataList.Count);

        // Let ComputeShader do the calculation
        computeShader.Dispatch(kernel, inputBufferDataList.Count, 1, 1);

        // Receive data array
        resultBuffer.GetData(resultBufferDataArray);

        // Add force for each Rigidbodies
        for (int i = 0; i < resultBufferDataArray.Length; i++)
        { 
            TargetsList[i].AddForce(resultBufferDataArray[i].force, ForceMode.Force);
        }

        // Empty the elements of the used list
        inputBufferDataList.Clear();
    }

    private void OnDestroy() 
    {
        inputBuffer.Release();
        resultBuffer.Release();
    }
}


