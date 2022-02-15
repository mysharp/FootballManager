using System;

namespace FootballManager
{
    public class PhysicalExmination : IPhysicalExmination
    {
        public event EventHandler HealthChecked;

        public IMedicalRoom MedicalRoom
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public PhysicalGrade PhysicalGrade
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public bool IsHealthy(int age, int strength, int speed)
        {
            throw new NotImplementedException();
        }

        public void IsHealthy(int age, int strength, int speed, out bool isHealthy)
        {
            throw new NotImplementedException();
        }
    }
}
