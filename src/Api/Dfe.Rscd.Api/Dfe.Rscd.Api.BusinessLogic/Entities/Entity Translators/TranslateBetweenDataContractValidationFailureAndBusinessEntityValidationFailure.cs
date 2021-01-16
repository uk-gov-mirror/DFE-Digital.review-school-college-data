using System;
using System.Collections.Generic;
using Web09.Checking.Business.Logic.Validation;
using Web09.Services.DataContracts;

namespace Web09.Services.Adapters.EntityTranslators
{
    public class TranslateBetweenDataContractValidationFailureAndBusinessEntityValidationFailure
    {
        public static List<Web09.Checking.Business.Logic.Validation.Student.ValidationFailure> TranslateDataContractValidationFailureListToBusinessEntityValidationFailureList(ValidationFailureList validationFailureListIn)
        {
            List<Web09.Checking.Business.Logic.Validation.Student.ValidationFailure> validationListOut = new List<Web09.Checking.Business.Logic.Validation.Student.ValidationFailure>();

            foreach(ValidationFailure validationFailure in validationFailureListIn)
            {
                validationListOut.Add(TranslateDataContractValidationFailureToBusinessEntityValidationFailure(validationFailure));
            }

            return validationListOut;

        }

        private static Web09.Checking.Business.Logic.Validation.Student.ValidationFailure TranslateDataContractValidationFailureToBusinessEntityValidationFailure(ValidationFailure validationFailureIn)
        {
            Web09.Checking.Business.Logic.Validation.Student.ValidationFailure validationFailureOut  = new Student.ValidationFailure
            {
                Message = validationFailureIn.Message, 
                PupilField = (Web09.Checking.Business.Logic.Validation.Student.PupilFieldEnum)
                    Enum.Parse(typeof(Web09.Checking.Business.Logic.Validation.Student.PupilFieldEnum), validationFailureIn.PupilFieldEnum, true)
            };

            return validationFailureOut;
        }
    }
}
