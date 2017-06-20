using System.Collections.Generic;
using Symlconnect.DataModel;

namespace Symlconnect.Maternity.Common
{
    /// <summary>
    /// Functionality to load a Patient and related Entities.
    /// </summary>
    public interface IPatientLoader
    {
        IEnumerable<IPatient> LoadPatients();
        IEnumerable<IEntity> LoadPatientEntities(IPatient patient, string entityName);
    }
}