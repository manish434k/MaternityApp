using System;

namespace Symlconnect.Common
{
    public static class DateTimeHelpers
    {
        public static int CalculateAge(DateTime dateOfBirth, DateTime referenceDate)
        {
            var age = referenceDate.Year - dateOfBirth.Year;
            if (dateOfBirth > referenceDate.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        public static DateTime CalculateDueDate(DateTime LMPDate)
        {
            DateTime Duedate = DateTime.Today;
            try
            {
                Duedate = LMPDate.AddDays(208);
            }
            catch (Exception ex)
            {
            }
            return Duedate;
        }
    }
}