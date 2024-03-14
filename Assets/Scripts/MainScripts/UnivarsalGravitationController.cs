using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class UnivarsalGravitationController : MonoBehaviour
{
    [Tooltip("Please add the objects to be subjected to universal gravitation to the list by dragging and dropping them from the Hierarchy view.")]
    [SerializeField]
    public List<Rigidbody> GravityTargets_obj = new List<Rigidbody>();
    [Tooltip("全てのRigidbodyをリストに追加する場合はチェックをして下さい")]
    public bool AddAllRigidbody;

    // Coefficient to multiply for a given force (to visible easily)
    [SerializeField]
    [Range(1, 100000000000)] // Range is able to change if necessary
    public float COEFFICIENT;


    // Universal constant of gravitation
    [HideInInspector]
    public float CONSTANT;        
    

    void Awake()
    {
        // Add Rigidbodies to the list
        Rigidbody[] rblist = FindObjectsOfType<Rigidbody>();
            GravityTargets_obj = GravityTargets_obj.Union(rblist).ToList();

        // Calculate universal constant of gravitation
        CONSTANT = 6.674f * Mathf.Pow(10, -11);

        // Disable useGravity on all Rigidbodies
        for (int i = 0; i <= GravityTargets_obj.Count -1 ; i++){
            GravityTargets_obj[i].useGravity = false;
        }
    }


    void FixedUpdate()
    { 

        for (int i = 0; i <= GravityTargets_obj.Count -1 ;)
        {
            for(int n = 0; n <= GravityTargets_obj.Count -1 ;)
            {
                if (i != n)
                {
                    // Get the square of the distance between two points
                    Vector3 direction = GravityTargets_obj[i].transform.position - GravityTargets_obj[n].transform.position;
                    float distance = direction.magnitude;
                    distance *= distance;

                    // Calculate universal gravitation
                    float gravity = CONSTANT * GravityTargets_obj[i].mass * GravityTargets_obj[n].mass / distance;

                    // Add force
                    GravityTargets_obj[i].AddForce(gravity * direction.normalized * -1 * COEFFICIENT, ForceMode.Force);
                    
                }
                n++;
            }
            i++;
        }
    }
}


