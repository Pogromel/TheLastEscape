using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private CharacterController characterController;
    
    
    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private Vector2 cameraRotation = Vector2.zero;
    
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintStaminaReduction;
    [SerializeField] private float staminaRecoveryRate;
    [SerializeField] private float maxStamina;
    [SerializeField] private int gravityForce;
    private float currentStamina;
    private Vector3 playerVelocity;
    
    [Header("Head Bob Settings")]
    [SerializeField] private bool isHeadBobEnabled;
    [SerializeField] private Vector3 cameraStartLocalPosition;
    [SerializeField] private float headBobAmplitude;
    [SerializeField] private float headBobFrequency;
    [SerializeField] private float returnSpeed;
    

    [Header("Lean Settings")] 
    [SerializeField] private float leanDistance;
    [SerializeField] private float leanHeightOffset ;
    [SerializeField] private float leanAngle;
    [SerializeField] private float leanSpeed;
    private Vector3 currentLeanPosition;
    private Quaternion currentLeanRotation;
    private bool isLeaning;
    
    private Transform characterTransform;


    void Start()
    {
        cameraStartLocalPosition = cameraTransform.localPosition;
        currentLeanPosition = cameraStartLocalPosition;
        currentLeanRotation = cameraTransform.localRotation;
        currentStamina = maxStamina;
        
        characterTransform = characterController.transform;
    }


     void Update()
    {
        Movement();
        Looking();
        FellThroughWorldCheck();
        Leaning();
        ApplyHeadBob();
        
    }

    void Movement()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3(moveX, 0, moveZ);
        move = characterTransform.TransformDirection(move);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        float speed = isSprinting ? sprintSpeed : movementSpeed;

        if (isSprinting)
        {
            currentStamina -= sprintStaminaReduction * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        move *= speed;

        if (characterController.isGrounded)
        {
            playerVelocity.y = -0.5f;
        }
        else
        {
            playerVelocity.y -= gravityForce * Time.deltaTime;
        }

        playerVelocity.x = move.x;
        playerVelocity.z = move.z;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void Looking()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseMovement = mouseMovement * cameraSensitivity;
        
        cameraRotation.x += mouseMovement.x;
        cameraRotation.y -= mouseMovement.y;
        
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -cameraRotationLimit, cameraRotationLimit);
        characterTransform.localRotation = Quaternion.Euler(0, cameraRotation.x, 0);
        cameraTransform.localRotation = Quaternion.Euler(cameraRotation.y, 0, 0) * currentLeanRotation;
        
        
    }
    
    private void FellThroughWorldCheck()
    {
        if (transform.position.y <= -100)
        {
            transform.position = new Vector3(transform.position.x,50f,transform.position.z);
        }
    }

    private void Leaning()
    {
        Vector3 targetLeanPosition = cameraStartLocalPosition;
        Quaternion targetLeanRotation = Quaternion.identity;
        isLeaning = false;

        if (Input.GetKey(KeyCode.Q))
        {
            targetLeanPosition += Vector3.left * leanDistance;
            targetLeanPosition -= Vector3.up * leanHeightOffset;
            targetLeanRotation = Quaternion.Euler(0, 0, leanAngle);
            isLeaning = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetLeanPosition += Vector3.right * leanDistance;
            targetLeanPosition -= Vector3.up * leanHeightOffset;
            targetLeanRotation = Quaternion.Euler(0, 0, -leanAngle);
            isLeaning = true;
        }

        currentLeanPosition = Vector3.Lerp(currentLeanPosition, targetLeanPosition, Time.deltaTime * leanSpeed);
        currentLeanRotation = Quaternion.Lerp(currentLeanRotation, targetLeanRotation, Time.deltaTime * leanSpeed);
    
  
        cameraTransform.localPosition = currentLeanPosition;
     
        cameraTransform.localRotation = Quaternion.Euler(cameraRotation.y, 0, 0) * currentLeanRotation;
    }
    
    void ApplyHeadBob()
    {
        if (!isLeaning && isHeadBobEnabled && new Vector2(playerVelocity.x, playerVelocity.z).magnitude != 0)
        {
            Vector3 headBobOffset = new Vector3(0f, Mathf.Sin(Time.time * headBobFrequency) * headBobAmplitude, 0f);
            cameraTransform.localPosition += headBobOffset * Time.deltaTime;
        }
        else
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraStartLocalPosition, Time.deltaTime * returnSpeed);
        }
    }
    
  
    



}
