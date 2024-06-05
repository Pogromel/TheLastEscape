using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    
    [Header("Character Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;


    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private Vector2 cameraRotation = Vector2.zero;


    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintChange;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float sprintStaminaReduction;


    [Header("Head Bob Settings")]
    [SerializeField] private bool isHeadBobEnabled;
    [SerializeField] private Vector3 cameraStartLocalPosition;
    [SerializeField] private float headBobAmplitude;
    [SerializeField] private float headBobFrequency;
    [SerializeField] private float returnSpeed;



    void Start()
    {
        cameraStartLocalPosition = cameraTransform.localPosition;
        
    }


     void Update()
    {
        Movement();
        Looking();
        FellThroughWorldCheck();
    }

    void Movement()
    {
        playerVelocity.z = Input.GetAxis("Vertical");
        playerVelocity.x = Input.GetAxis("Horizontal");
        
        playerVelocity *=  movementSpeed * Time.deltaTime;
        playerVelocity = characterTransform.TransformDirection(playerVelocity); 
        
    }
    void Looking()
    {
       
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMovement = mouseMovement * cameraSensitivity;
        cameraRotation.x += mouseMovement.x;
        cameraRotation.y += mouseMovement.y;
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -cameraRotationLimit, cameraRotationLimit);
        var xQuat = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(cameraRotation.y, Vector3.left);

        cameraTransform.localRotation = yQuat;
        transform.localRotation = xQuat;
        if (isHeadBobEnabled && new Vector2(playerVelocity.x, playerVelocity.z).magnitude != 0)
        {
            Vector3 headBobber = new Vector3(0f, 0f, 0f);
            headBobber.y += Mathf.Sin(Time.time * headBobFrequency);
            headBobber *= headBobAmplitude * Time.deltaTime;
            cameraTransform.localPosition += headBobber;
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraStartLocalPosition, Time.deltaTime * returnSpeed);
        
    }
    
    private void FellThroughWorldCheck()
    {
        if (transform.position.y <= -100)
        {
            transform.position = new Vector3(transform.position.x,50f,transform.position.z);
        }
    }
    



}
