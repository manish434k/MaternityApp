using Symlconnect.DataModel;

namespace Symlconnect.Maternity.Common
{
    public interface IPatientEntityPersistence
    {
        void PersistPatientEntity(IPatient patient, IEntity entity);
    }
}