using Unity.VisualScripting;
using UnityEngine;

namespace EasingToolkit
{
    public static class EasingTools
    {
        /// <summary>
        /// Applies an Easing process to a value.
        /// <para>Input values must be within the Range [0,1] (inclusive).</para>
        /// Output values might not be within that range.
        /// </summary>
        /// <param name="x">Value to ease. <para>Must be within the Range [0,1] (inclusive).</para></param>
        /// <param name="typeOfEase">The types of Easing availible can be visualized at https://easings.net/.</param>
        /// <returns></returns>
        public static float ApplyEase(float x, EaseType typeOfEase)
        {
            // Checking the range of the input.
            if (x < 0 || x > 1)
            {
                Debug.LogWarning("Easing function of type [" + typeOfEase.DisplayName() + "] failed, returning 0." +
                    "\nParameter x == [" + x + "], but x must be in the (0,1) range (inclusive).");
                return 0;
            }

            switch (typeOfEase)
            {
                // Sine functions:
                case EaseType.InSine: return InSine(x);
                case EaseType.OutSine: return OutSine(x);
                case EaseType.InOutSine: return InOutSine(x);

                // Quad functions:
                case EaseType.InQuad: return InQuad(x);
                case EaseType.OutQuad: return OutQuad(x);
                case EaseType.InOutQuad: return InOutQuad(x);

                // Cubic functions:
                case EaseType.InCubic: return InCubic(x);
                case EaseType.OutCubic: return OutCubic(x);
                case EaseType.InOutCubic: return InOutCubic(x);

                // Quart functions:
                case EaseType.InQuart: return InQuart(x);
                case EaseType.OutQuart: return OutQuart(x);
                case EaseType.InOutQuart: return InOutQuart(x);

                // Quint functions:
                case EaseType.InQuint: return InQuint(x);
                case EaseType.OutQuint: return OutQuint(x);
                case EaseType.InOutQuint: return InOutQuint(x);

                // Expo functions:
                case EaseType.InExpo: return InExpo(x);
                case EaseType.OutExpo: return OutExpo(x);
                case EaseType.InOutExpo: return InOutExpo(x);

                // Circ functions:
                case EaseType.InCirc: return InCirc(x);
                case EaseType.OutCirc: return OutCirc(x);
                case EaseType.InOutCirc: return InOutCirc(x);

                // Back functions:
                case EaseType.InBack: return InBack(x);
                case EaseType.OutBack: return OutBack(x);
                case EaseType.InOutBack: return InOutBack(x);

                // Elastic functions:
                case EaseType.InElastic: return InElastic(x);
                case EaseType.OutElastic: return OutElastic(x);
                case EaseType.InOutElastic: return InOutElastic(x);

                // Bounce functions:
                case EaseType.InBounce: return InBounce(x);
                case EaseType.OutBounce: return OutBounce(x);
                case EaseType.InOutBounce: return InOutBounce(x);
            }


            // If the switch does not return anything, something has gone wrong.
            Debug.LogWarning("Type of ease [" + typeOfEase.ToString() + "] not recognized in ApplyEase()'s code. " +
                             "Returning 0.");
            return 0;
        }


        // These are all of the possible Easing processes.
        // They can be visualized at https://easings.net/

        /// <summary>
        /// The different Easing types processes possible. They can be visualized at https://easings.net/.
        /// </summary>
        public enum EaseType
        {
            InSine, OutSine, InOutSine,
            InQuad, OutQuad, InOutQuad,
            InCubic, OutCubic, InOutCubic,
            InQuart, OutQuart, InOutQuart,
            InQuint, OutQuint, InOutQuint,
            InExpo, OutExpo, InOutExpo,
            InCirc, OutCirc, InOutCirc,
            InBack, OutBack, InOutBack,
            InElastic, OutElastic, InOutElastic,
            InBounce, OutBounce, InOutBounce
        }

        // Sine functions.
        static float InSine(float x) => 1 - Mathf.Cos((Mathf.PI * x) / 2);
        static float OutSine(float x) => Mathf.Sin((Mathf.PI * x) / 2);
        static float InOutSine(float x) => -(Mathf.Cos(Mathf.PI * x) - 1) / 2;


        // Quad functions.
        static float InQuad(float x) => x * x;
        static float OutQuad(float x) => 1 - InQuad(1 - x);
        static float InOutQuad(float x)
        {
            if (x < 0.5)
                return InQuad(x * 2) / 2;
            else
                return 1 - InQuad((1 - x) * 2) / 2;
        }


        // Cubic functions.
        static float InCubic(float x) => x * x * x;
        static float OutCubic(float x) => 1 - InCubic(1 - x);
        static float InOutCubic(float x)
        {
            if (x < 0.5)
                return InCubic(x * 2) / 2;
            else
                return 1 - InCubic((1 - x) * 2) / 2;
        }


        // Quart functions.
        static float InQuart(float x) => x * x * x * x;
        static float OutQuart(float x) => 1 - InQuart(1 - x);
        static float InOutQuart(float x)
        {
            if (x < 0.5)
                return InQuart(x * 2) / 2;
            else
                return 1 - InQuart((1 - x) * 2) / 2;
        }


        // Quint functions.
        static float InQuint(float x) => x * x * x * x * x;
        static float OutQuint(float x) => 1 - InQuint(1 - x);
        static float InOutQuint(float x)
        {
            if (x < 0.5)
                return InQuint(x * 2) / 2;
            else
                return 1 - InQuint((1 - x) * 2) / 2;
        }


        // Expo functions.
        static float InExpo(float x) => Mathf.Pow(2, 10 * (x - 1));
        static float OutExpo(float x) => 1 - InExpo(1 - x);
        static float InOutExpo(float x)
        {
            if (x < 0.5)
                return InExpo(x * 2) / 2;
            else
                return 1 - InExpo((1 - x) * 2) / 2;
        }


        // Circ functions.
        static float InCirc(float x) => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
        static float OutCirc(float x) => 1 - InCirc(1 - x);
        static float InOutCirc(float x)
        {
            if (x < 0.5)
                return InCirc(x * 2) / 2;
            else
                return 1 - InCirc((1 - x) * 2) / 2;
        }


        // Back functions.
        static float InBack(float x) => x * x * ((1.70158f + 1) * x - 1.70158f);
        static float OutBack(float x) => 1 - InBack(1 - x);
        static float InOutBack(float x)
        {
            if (x < 0.5)
                return InBack(x * 2) / 2;
            else
                return 1 - InBack((1 - x) * 2) / 2;
        }


        // Elastic functions.
        static float InElastic(float x) => 1 - OutElastic(1 - x);
        static float OutElastic(float x) => Mathf.Pow(2, -10 * x) * Mathf.Sin((x - 0.3f / 4) * (2 * Mathf.PI) / 0.3f) + 1;
        static float InOutElastic(float x)
        {
            if (x < 0.5)
                return InElastic(x * 2) / 2;
            else
                return 1 - InElastic((1 - x) * 2) / 2;
        }


        // Bounce functions.
        static float InBounce(float x) => 1 - OutBounce(1 - x);
        static float OutBounce(float x)
        {
            if (x < 1 / 2.75f)
            {
                return 7.5625f * x * x;
            }
            else if (x < 2 / 2.75f)
            {
                x -= 1.5f / 2.75f;
                return 7.5625f * x * x + 0.75f;
            }
            else if (x < 2.5 / 2.75f)
            {
                x -= 2.25f / 2.75f;
                return 7.5625f * x * x + 0.9375f;
            }
            else
            {
                x -= 2.625f / 2.75f;
                return 7.5625f * x * x + 0.984375f;
            }
        }
        static float InOutBounce(float x)
        {
            if (x < 0.5)
                return InBounce(x * 2) / 2;
            else
                return 1 - InBounce((1 - x) * 2) / 2;
        }
    }
}