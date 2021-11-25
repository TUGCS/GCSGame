using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Justinas
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float acceleration, deceleration, steering;
        [SerializeField] private InputActionReference accelerateAction, brakeAction, handBrakeAction, steerAction;
        private Rigidbody _rb;
        private bool _accelerating, _braking, _handBraking;
        private float _steer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            accelerateAction.action.performed += OnAccelerate;
            accelerateAction.action.canceled += OnAccelerate;
            brakeAction.action.performed += OnBreak;
            brakeAction.action.canceled += OnBreak;
            handBrakeAction.action.performed += OnHandBrake;
            handBrakeAction.action.canceled += OnHandBrake;
            steerAction.action.performed += OnSteer;
            steerAction.action.canceled += OnSteer;
        }

        private void OnEnable()
        {
            accelerateAction.action.Enable();
            brakeAction.action.Enable();
            handBrakeAction.action.Enable();
            steerAction.action.Enable();
        }

        private void OnDisable()
        {
            accelerateAction.action.Disable();
            brakeAction.action.Disable();
            handBrakeAction.action.Disable();
            steerAction.action.Disable();
        }

        private void FixedUpdate()
        {
            var force = Vector3.zero;
            var torque = Vector3.zero;
            var t = transform;
            if (_accelerating)
            {
                torque += t.up * steering * _steer;
                force += t.forward * acceleration;
            }
            if (_braking)
            {
                torque -= t.up * steering * _steer;
                force -= t.forward * deceleration;
            }
            _rb.AddTorque(torque);
            _rb.AddForce(force);
        }

        private void OnAccelerate(InputAction.CallbackContext obj)
        {
            _accelerating = obj.performed;
        }

        private void OnBreak(InputAction.CallbackContext obj)
        {
            _braking = obj.performed;
        }

        private void OnHandBrake(InputAction.CallbackContext obj)
        {
            _handBraking = obj.performed;
        }

        private void OnSteer(InputAction.CallbackContext obj)
        {
            _steer = obj.ReadValue<float>();
        }
    }
}
