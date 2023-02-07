using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Validations.Models
{
    public static class ValidationExternsionMethods
    {

        public static void RequireObject(this List<ValidationKeyValue> validations, string fieldName, object value)
        {
            if (validations == null) validations = new List<ValidationKeyValue>();

            if (value == null) validations.Add(new ValidationKeyValue(fieldName, $"Campul '{fieldName}' este invalid!"));
        }
        public static void ValidateString(this List<ValidationKeyValue> validations, string fieldName, string value, int min, int max)
        {
            if (validations == null) validations = new List<ValidationKeyValue>();

            if (value.Length < min || value.Length > max) validations.Add(ValidationKeyValue.InvalidString(fieldName, min, max));
        }

        public static void ValidateStringChars(this List<ValidationKeyValue> validations, string fieldName, string value, string requiredChars, int noOfReqChars)
        {
            if (validations == null) validations = new List<ValidationKeyValue>();

            int counter = 0;
            foreach(var s in requiredChars)
            {
                if (value.Contains(s)) counter++;
            }
            if(counter < noOfReqChars) validations.Add(ValidationKeyValue.InvalidStringChars(fieldName, requiredChars, noOfReqChars));
        }

        public static void ValidateNumber(this List<ValidationKeyValue> validations, string fieldName, int value, int min, int max)
        {
            if (validations == null) validations = new List<ValidationKeyValue>();

            if (value < min || value > max) validations.Add(ValidationKeyValue.InvalidNumber(fieldName, min, max));
        }

        public static void ValidateDate(this List<ValidationKeyValue> validations, string fieldName, DateTime value, DateTime min, DateTime max)
        {
            if (validations == null) validations = new List<ValidationKeyValue>();

            if (value < min || value > max) validations.Add(ValidationKeyValue.InvalidDate(fieldName, min, max));
        }

    }
}
