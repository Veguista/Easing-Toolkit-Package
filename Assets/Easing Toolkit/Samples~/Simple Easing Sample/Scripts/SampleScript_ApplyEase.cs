using UnityEngine;
using EasingToolkit;

namespace EasingToolkit.Samples
{
    public class SampleScript_ApplyEase : MonoBehaviour
    {
        [Header("Type of ease")]
        public EaseType easeType = EaseType.InSine;

        [Header("References")]
        public Transform uneasedMovementTransform;
        public Transform easedMovementTransform;

        [Header("Movement Configuration")]
        [SerializeField][Range(0.1f, 10)] float maxDuration = 1.5f;
        [SerializeField][Range(0.1f, 10)] float displacementQuantity = 5.0f;
        [SerializeField][Range(1, 3)] int positionOfStart = 1;

        float delayTime = 0;
        float time = 0;

        private void Start()
        {
            switch (positionOfStart)
            {
                case 1:
                    break;

                case 2:
                    time = maxDuration;
                    delayTime = maxDuration * (4 - 1.2f);
                    break;

                case 3:
                    time = maxDuration;
                    delayTime = maxDuration * (4 - 1.2f * 2);
                    break;

                default:
                    Debug.LogError("Position of Start parameter cannot be set to a value outside the [1, 3] range.");
                    break;
            }
        }

        void Update()
        {
            if(time >= maxDuration)
            {
                delayTime += Time.deltaTime;

                if (delayTime >= maxDuration * 4)
                {
                    delayTime = 0;
                    time = 0;
                }
            }
            else
            {
                time += Time.deltaTime;

                if (time > maxDuration)
                {
                    time = maxDuration;
                }

                float percentageComplete = Mathf.Clamp01(time / maxDuration);

                float easedPercentageComplete = Easing.ApplyEase(percentageComplete, easeType);

                ApplyDisplacementToTransform(uneasedMovementTransform, percentageComplete);
                ApplyDisplacementToTransform(easedMovementTransform, easedPercentageComplete);
            }
        }
        
        void ApplyDisplacementToTransform(Transform _target, float _percentageOfMovement)
        {
            _target.transform.localPosition = Vector3.LerpUnclamped(Vector3.zero, Vector3.right * displacementQuantity, _percentageOfMovement);
        }
    }
}
