using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Justinas
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float torque, brakeTorque, turnAngle;
        [SerializeField] private InputActionReference accelerateAction, handBrakeAction, steerAction;
        [SerializeField] private List<AxleInfo> axles = new List<AxleInfo>();
        private Rigidbody _rb;
        private bool _handBraking;
        private float _acceleration, _steer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            accelerateAction.action.performed += OnAccelerate;
            accelerateAction.action.canceled += OnAccelerate;
            handBrakeAction.action.performed += OnHandBrake;
            handBrakeAction.action.canceled += OnHandBrake;
            steerAction.action.performed += OnSteer;
            steerAction.action.canceled += OnSteer;
        }

        private void OnEnable()
        {
            accelerateAction.action.Enable();
            handBrakeAction.action.Enable();
            steerAction.action.Enable();
        }

        private void OnDisable()
        {
            accelerateAction.action.Disable();
            handBrakeAction.action.Disable();
            steerAction.action.Disable();
        }

        private void FixedUpdate()
        {
            float brake = 0f,
                torquePower = torque * _acceleration,
                steerAngle = turnAngle * _steer;
            if (_handBraking) {
                brake = brakeTorque;
                torquePower = 0;
            }
            foreach (var axle in axles)
            {
                if (axle.motor)
                {
                    axle.left.brakeTorque = brake;
                    axle.right.brakeTorque = brake;
                    axle.left.motorTorque = torquePower;
                    axle.right.motorTorque = torquePower;
                }
                if (axle.steering)
                {
                    axle.left.steerAngle = steerAngle;
                    axle.right.steerAngle = steerAngle;
                }
            }
        }

        private void OnAccelerate(InputAction.CallbackContext obj)
        {
            _acceleration = obj.ReadValue<float>();
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
