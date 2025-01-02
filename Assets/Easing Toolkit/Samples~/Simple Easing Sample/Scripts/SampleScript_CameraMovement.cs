using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasingToolkit.Samples
{
    public class SampleScript_CameraMovement : MonoBehaviour
    {
        [SerializeField] float velocity = 5;

        [Header("Limits")]
        [SerializeField] float upperCameraLimit = 10;
        [SerializeField] float lowerCameraLimit = -10;

        // Update is called once per frame
        void Update()
        {
            float input = 0;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                input += 1;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                input -= 1;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, 
                transform.position.z + velocity * input * Time.deltaTime);
        }
    }
}
