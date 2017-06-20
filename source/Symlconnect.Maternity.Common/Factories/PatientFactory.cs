using Symlconnect.Contracts.Factories;

namespace Symlconnect.Maternity.Common.Factories
{
    public class PatientFactory : IFactory<IPatient>
    {
        public IPatient CreateInstance()
        {
            return new Patient();
        }
    }
}