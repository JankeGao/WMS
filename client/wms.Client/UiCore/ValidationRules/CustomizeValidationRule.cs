﻿using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using wms.Client.LogicCore.Enums;
using wms.Client.LogicCore.UserAttribute;

namespace wms.Client.UiCore.ValidationRules
{
    public class CustomizeValidationRule : ValidationRule
    {
        public ValidationType validationType { get; set; } = ValidationType.None;
        public string errorMessage { get; set; } = string.Empty;
        public int minLength { get; set; }
        public int maxLength { get; set; }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string regex = string.Empty;
            if (validationType != ValidationType.None && validationType != ValidationType.Str)
                regex = GetEnumAttrbute.GetDescription(validationType).Caption;

            if (!string.IsNullOrWhiteSpace(regex))
            {
                Regex re = new Regex(regex);
                string input = (value ?? "").ToString();
                if (re.IsMatch(input))
                {
                    return ValidationResult.ValidResult;
                }
                else
                    return new ValidationResult(false, errorMessage);
            }
            else
            {
                string input = (value ?? "").ToString();
                int length = Encoding.Default.GetByteCount(input);
                if (length < minLength || length > maxLength)
                {
                    return new ValidationResult(false, errorMessage);
                }
                return ValidationResult.ValidResult;
            }
        }
    }
}
