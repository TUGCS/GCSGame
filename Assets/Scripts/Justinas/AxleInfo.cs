using System;
using UnityEngine;

namespace Justinas
{
    [Serializable]
    public struct AxleInfo
    {
        public WheelCollider left, right;
        public bool motor, steering;
    }
}