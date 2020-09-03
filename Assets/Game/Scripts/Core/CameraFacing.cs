using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class CameraFacing : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
