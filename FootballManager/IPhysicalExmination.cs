using System;

namespace FootballManager
{
    public interface IPhysicalExmination
    {
        bool IsHealthy(int age, int strength, int speed);

        void IsHealthy(int age, int strength, int speed, out bool isHealthy);

        IMedicalRoom MedicalRoom { get; set; }

        PhysicalGrade PhysicalGrade { get; set; }

        event EventHandler HealthChecked;
    }

    public interface IMedicalRoom
    {
        IMedicalRoomStatus Status { get; set; }
    }

    public interface IMedicalRoomStatus
    {
        bool IsAvailable { get; set; }
    }
}
