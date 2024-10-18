using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WNC.HYPEPOLY
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 direction;
        public bool localAxis = true;

        private void FixedUpdate()
        {
            transform.Rotate(direction, localAxis ? Space.Self : Space.World);
        }
    }
}