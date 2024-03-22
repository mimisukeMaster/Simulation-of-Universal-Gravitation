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
    [Tooltip("全てのRigidbodyをリストに追加する場合はチェックをして下さい")]
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

    private List<InputBufferData> inputBufferDataList  = new List<InputBufferData>();

    private List<ResultBufferData> initialBufferDataList = new List<ResultBufferData>();
    private ResultBufferData[] resultBufferDataArray;

    // kernel保持
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

        // Disable useGravity on all Rigidbodies and create a list by also assigning to a structure
        for (int i = 0; i < TargetsList.Count; i++) TargetsList[i].useGravity = false;

        // Initialisation of compute buffer

        // 各Bufferの用意をここでする、要素数と型はわかりきっているので.
        // 初期化しないと下ののSetBuffer(  )でエラーが出る
        // 第一引数はこのあと_input_bufferに入れるListの要素数分
        _input_buffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(InputBufferData)));
        _result_buffer = new ComputeBuffer(TargetsList.Count, Marshal.SizeOf(typeof(ResultBufferData)));

        // Set buffers in compute shader 
        _kernel = _computeShader.FindKernel("UGCalc");
        _computeShader.SetBuffer(_kernel, "InputBuffer", _input_buffer);
        _computeShader.SetBuffer(_kernel, "ResultBuffer", _result_buffer);
        
        // Set values
        _computeShader.SetFloat("constant", CONSTANT);
        _computeShader.SetFloat("coefficient",COEFFICIENT);
        _computeShader.SetInt("list_count", TargetsList.Count);

        // Prepare as many elements of the list as are needed to assign the data to
        for (int i = 0; i < TargetsList.Count; i++){
            initialBufferDataList.Add(new ResultBufferData());
        }
        resultBufferDataArray = new ResultBufferData[TargetsList.Count];
    }


    void FixedUpdate()
    {
        // The position changes, so they need to be sent every frame

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

        // inputBufferDataListだけだと要素数分が確保できない？いくら分使うのか示す必要があるから第3引数要る？
        // _result_buffer にはNewされただけのListをSetして向こうのResultBufferの要素数を確保
        _input_buffer.SetData(inputBufferDataList, 0, 0, inputBufferDataList.Count);
        _result_buffer.SetData(initialBufferDataList, 0, 0, initialBufferDataList.Count);


        _computeShader.Dispatch(_kernel,inputBufferDataList.Count,1,1);

        // Get data array
        _result_buffer.GetData(resultBufferDataArray);

        // Empty the elements of the used list
        for (int i = 0; i < resultBufferDataArray.Length; i++){ 
            TargetsList[i].AddForce(resultBufferDataArray[i].force, ForceMode.Force);
        }

        // 最後に使用済Listを空にする init buffer list はもともとNewしてある
        inputBufferDataList.Clear();
    }
    private void OnDestroy() 
    {
        _input_buffer.Release();
        _result_buffer.Release();
    }
}


