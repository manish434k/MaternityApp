using System;

namespace Symlconnect.Maternity.Common
{
    public interface IPatient
    {
        string Id { get; set; }
        string Name { get; set; }
        string PatientNumber { get; set; }
        DateTime? DateOfBirth { get; set; }
    }
}