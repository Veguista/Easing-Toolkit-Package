using UnityEngine;

namespace EasingToolkit
{
    public static class Easing
    {
        /// <summary>
        /// Applies an Easing process to a value.
        /// <para>Input values must be within the Range [0,1] (inclusive).</para>
        /// Output values might not be within that range.
        /// </summary>
        /// <param name="inputFloat">Value to ease. <para>Must be within the Range [0,1] (inclusive).</para></param>
        /// <param name="typeOfEase">The types of Easing availible can be visualized at https://easings.net/.</param>
        /// <returns></returns>
        public static float ApplyEase(float inputFloat, EaseType typeOfEase)
        {
            // Checking the range of the input.
            if (inputFloat < 0 || inputFloat > 1)
            {
                Debug.LogWarning("Easing function of type [" + typeOfEase.ToString() + "] failed, returning 0." +
                    "\nParameter inputFloat == [" + inputFloat + "], but inputFloat must be in the (0,1) range (inclusive).");
                return 0;
            }

            switch (typeOfEase)
            {
                // Sine functions:
                case EaseType.InSine: return InSine(inputFloat);
                case EaseType.OutSine: return OutSine(inputFloat);
                case EaseType.InOutSine: return InOutSine(inputFloat);

                // Quad functions:
                case EaseType.InQuad: return InQuad(inputFloat);
                case EaseType.OutQuad: return OutQuad(inputFloat);
                case EaseType.InOutQuad: return InOutQuad(inputFloat);

                // Cubic functions:
                case EaseType.InCubic: return InCubic(inputFloat);
                case EaseType.OutCubic: return OutCubic(inputFloat);
                case EaseType.InOutCubic: return InOutCubic(inputFloat);

                // Quart functions:
                case EaseType.InQuart: return InQuart(inputFloat);
                case EaseType.OutQuart: return OutQuart(inputFloat);
                case EaseType.InOutQuart: return InOutQuart(inputFloat);

                // Quint functions:
                case EaseType.InQuint: return InQuint(inputFloat);
                case EaseType.OutQuint: return OutQuint(inputFloat);
                case EaseType.InOutQuint: return InOutQuint(inputFloat);

                // Expo functions:
                case EaseType.InExpo: return InExpo(inputFloat);
                case EaseType.OutExpo: return OutExpo(inputFloat);
                case EaseType.InOutExpo: return InOutExpo(inputFloat);

                // Circ functions:
                case EaseType.InCirc: return InCirc(inputFloat);
                case EaseType.OutCirc: return OutCirc(inputFloat);
                case EaseType.InOutCirc: return InOutCirc(inputFloat);

                // Back functions:
                case EaseType.InBack: return InBack(inputFloat);
                case EaseType.OutBack: return OutBack(inputFloat);
                case EaseType.InOutBack: return InOutBack(inputFloat);

                // Elastic functions:
                case EaseType.InElastic: return InElastic(inputFloat);
                case EaseType.OutElastic: return OutElastic(inputFloat);
                case EaseType.InOutElastic: return InOutElastic(inputFloat);

                // Bounce functions:
                case EaseType.InBounce: return InBounce(inputFloat);
                case EaseType.OutBounce: return OutBounce(inputFloat);
                case EaseType.InOutBounce: return InOutBounce(inputFloat);
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