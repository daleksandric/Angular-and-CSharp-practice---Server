﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourManagement.API.Helpers
{
    public class CustomizedValidationResult : Dictionary<string, IEnumerable<CustomizedValidationError>>
    {
        public CustomizedValidationResult() 
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public CustomizedValidationResult(ModelStateDictionary modelState) 
            : this()
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            // {
            //      { key, { err1, err2 } } 
            //      { key, { err3, err4 } }
            // }
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorsToAdd = new List<CustomizedValidationError>();
                    foreach (var error in errors)
                    {
                        //split the message to get the validator key
                        var keyAndMessage = error.ErrorMessage.Split('|');

                        //if there's no validator key, just return the error message,
                        // otherwise add the validatorkey
                        if (keyAndMessage.Count() > 1)      // for error message like this:  "required|The end date is required."
                        {
                            errorsToAdd.Add(new CustomizedValidationError(
                                keyAndMessage[1],
                                keyAndMessage[0]));
                        }
                        // for error message like this: "The end date is required."
                        else
                        {
                            errorsToAdd.Add(new CustomizedValidationError(
                                keyAndMessage[0]));
                        }
                    }
                    Add(key, errorsToAdd);
                }
            }
        }
    }
}
