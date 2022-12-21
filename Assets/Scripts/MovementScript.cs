using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementScript : MonoBehaviour
{

    [SerializeField]private float speed = 5f;
    [SerializeField]private PlayerTurn playerTurn;
    [SerializeField]private float mouseSensitivity;
    [SerializeField]private Transform playerCamera;
    private CharacterController _characterController;
    private bool _groundedPlayer;
    private Vector3 _playerVelocity;
    private float _playerJump = 0.5f;
    private float _gravityValue = -9.82f;
    private float _xRotation = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundedAndJump();
    }
    private void FixedUpdate()
    {
        if (playerTurn.IsPlayerTurn())
        {
            PlayerMove();
        }
    }

    private void LateUpdate()
    {
        FollowMouse();
    }

    private void PlayerMove()
    {
        Vector3 playerMove = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        _characterController.Move(playerMove * (Time.fixedDeltaTime * speed));
        _playerVelocity.y += _gravityValue * Time.fixedDeltaTime;
        _characterController.Move(_playerVelocity * Time.fixedDeltaTime);
    }
    private bool IsTouchingFloor()
    {
        RaycastHit hit;
        bool result =  Physics.SphereCast(transform.position, 0.15f, -transform.up, out hit, 1f);
        return result;
    }

    private void FollowMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
    
        _characterController.transform.Rotate(Vector3.up * mouseX);
    
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    
        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void GroundedAndJump()
    {
        _groundedPlayer = _characterController.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
            
        if (Input.GetButtonDown("Jump") && IsTouchingFloor())
        {
            _playerVelocity.y += Mathf.Sqrt(_playerJump * -3.0f * _gravityValue);
        }
    }

}
