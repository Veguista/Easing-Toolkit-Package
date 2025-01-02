using NUnit.Framework;
using EasingToolkit;

namespace EasingToolkit.Tests
{
    public class Tests_ApplyEase
    {
        [Test]
        public void T_EasingValueWhen_1()
        {
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InSine));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutSine));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutSine));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InQuad));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutQuad));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutQuad));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InCubic));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutCubic));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutCubic));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InQuart));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutQuart));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutQuart));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InQuint));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutQuint));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutQuint));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InExpo));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutExpo));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutExpo));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InCirc));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutCirc));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutCirc));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InBack));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutBack));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutBack));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InElastic));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutElastic));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutElastic));

            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InBounce));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.OutBounce));
            Assert.AreEqual(1, Easing.ApplyEase(1, EaseType.InOutBounce));
        }

        [Test]
        public void T_EasingValueWhen_0()
        {
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InSine));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutSine));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutSine));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InQuad));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutQuad));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutQuad));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InCubic));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutCubic));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutCubic));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InQuart));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutQuart));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutQuart));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InQuint));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutQuint));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutQuint));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InExpo));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutExpo));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutExpo));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InCirc));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutCirc));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutCirc));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InBack));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutBack));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutBack));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InElastic));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutElastic));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutElastic));

            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InBounce));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.OutBounce));
            Assert.AreEqual(0, Easing.ApplyEase(0, EaseType.InOutBounce));
        }

        [Test]
        public void T_EasingValueWhen_0_5f()
        {
            Assert.AreEqual(0.292893231f, Easing.ApplyEase(0.5f, EaseType.InSine));
            Assert.AreEqual(0.707106769f, Easing.ApplyEase(0.5f, EaseType.OutSine));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutSine));

            Assert.AreEqual(0.25f, Easing.ApplyEase(0.5f, EaseType.InQuad));
            Assert.AreEqual(0.75f, Easing.ApplyEase(0.5f, EaseType.OutQuad));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutQuad));

            Assert.AreEqual(0.125f, Easing.ApplyEase(0.5f, EaseType.InCubic));
            Assert.AreEqual(0.875f, Easing.ApplyEase(0.5f, EaseType.OutCubic));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutCubic));

            Assert.AreEqual(0.0625f, Easing.ApplyEase(0.5f, EaseType.InQuart));
            Assert.AreEqual(0.9375f, Easing.ApplyEase(0.5f, EaseType.OutQuart));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutQuart));

            Assert.AreEqual(0.03125f, Easing.ApplyEase(0.5f, EaseType.InQuint));
            Assert.AreEqual(0.96875f, Easing.ApplyEase(0.5f, EaseType.OutQuint));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutQuint));

            Assert.AreEqual(0.03125f, Easing.ApplyEase(0.5f, EaseType.InExpo));
            Assert.AreEqual(0.96875f, Easing.ApplyEase(0.5f, EaseType.OutExpo));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutExpo));

            Assert.AreEqual(0.133974612f, Easing.ApplyEase(0.5f, EaseType.InCirc));
            Assert.AreEqual(0.8660254f, Easing.ApplyEase(0.5f, EaseType.OutCirc));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutCirc));

            Assert.AreEqual(-0.087697506f, Easing.ApplyEase(0.5f, EaseType.InBack));
            Assert.AreEqual(1.08769751f, Easing.ApplyEase(0.5f, EaseType.OutBack));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutBack));

            Assert.AreEqual(-0.0156249879f, Easing.ApplyEase(0.5f, EaseType.InElastic));
            Assert.AreEqual(1.015625f, Easing.ApplyEase(0.5f, EaseType.OutElastic));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutElastic));

            Assert.AreEqual(0.234375f, Easing.ApplyEase(0.5f, EaseType.InBounce));
            Assert.AreEqual(0.765625f, Easing.ApplyEase(0.5f, EaseType.OutBounce));
            Assert.AreEqual(0.5f, Easing.ApplyEase(0.5f, EaseType.InOutBounce));
        }
    }
}
