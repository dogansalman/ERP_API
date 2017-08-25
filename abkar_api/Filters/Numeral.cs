using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace abkar_api.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class Numeral : ValidationAttribute
    {
  
        public override bool IsValid(object value)
        {
            if(String.IsNullOrEmpty(value.ToString())) return Regex.IsMatch((String)value, @"^\d+$");
            return true;

        }


        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}