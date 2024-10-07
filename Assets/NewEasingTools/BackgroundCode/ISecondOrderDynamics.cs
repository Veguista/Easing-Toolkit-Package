

namespace SecondOrderEasing
{
    public interface ISecondOrderDynamics
    {
        public void UpdateConstants(SO_Constants constants);
        public void Reset();
    }
}