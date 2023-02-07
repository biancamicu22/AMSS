using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Validations.Models
{
    public class ValidationKeyValue
    {
        public ValidationKeyValue()
        {
        }

        public ValidationKeyValue(string fieldName, string fieldError)
        {
            FieldName = fieldName;
            FieldError = fieldError;
        }

        public static ValidationKeyValue InvalidString(string fieldName, int min, int max) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"'{fieldName}' trebuie sa aiba intre {min} si {max} caractere!" };

        public static ValidationKeyValue InvalidStringChars(string fieldName, string requiredChars, int noOfReqChars) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"'{fieldName}' trebuie sa contina minim {noOfReqChars} caractere din '{requiredChars}'" };

        public static ValidationKeyValue InvalidNumber(string fieldName, int min, int max) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"'{fieldName}' trebuie sa fie intre {min} si {max}!" };

        public static ValidationKeyValue InvalidDate(string fieldName, DateTime min, DateTime max) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"'{fieldName}' trebuie sa fie intre {min} si {max}!" };

        public static ValidationKeyValue Duplicate(string fieldName) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"'{fieldName}' exista deja!" };

        public static ValidationKeyValue Invalid(string fieldName) =>
            new ValidationKeyValue { FieldName = fieldName, FieldError = $"Valoarea pentru campul '{fieldName}' nu este valida!" };


        public string FieldName { get; set; }

        public string FieldError { get; set; }
    }
}
