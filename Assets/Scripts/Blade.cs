using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public float minBladeVelocity = 0.01f;
    private Camera mainCamera;
    private Collider bladeCollider;
    private bool slicing;
    private TrailRenderer bladeTrail;
    public float sliceForce = 5.0f;
    public Vector3 direction {get; private set;}

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        bladeCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing) 
        {
            ContinueSlicing();
        }
    }

    // OnEnable() provides to add functions to gameobject while object active status is transform from passive(false) to active(true).
    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    public void StartSlicing()
    {
        // The position of blade is taken from clicked point on the screen.
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0.0f;
        transform.position = newPosition;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();

        
    }

    public void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.Clear();
        bladeTrail.enabled = false;
    }

    public void ContinueSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0.0f;

        direction = newPosition - transform.position;
        float velocity = direction.magnitude / Time.deltaTime;

        bladeCollider.enabled = velocity > minBladeVelocity;

        transform.position = newPosition;

    }

}
