using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MobileApplication.DataModel.QvDataAnnotation
{
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Required,
                ValidationType = "required"
            };
        }
    }

    public class RequiredIfAttribute : RequiredAttribute
    {
        public string FlagName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var prop = validationContext.ObjectInstance.GetType().GetProperty(FlagName);
            if (prop != null)
            {
                var propValue = (bool)prop.GetValue(validationContext.ObjectInstance);
                if (propValue)
                {
                    return base.IsValid(value,validationContext);
                }
                else
                {
                    return ValidationResult.Success;
                }

            }
            else
            {
                return ValidationResult.Success;
            }

        }
      
    }
    
    public class Email530Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Email530Attribute() : base(ValidationExpressions.revMail530) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Email,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
        
    public class EmailChar50Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
          public EmailChar50Attribute() : base(ValidationExpressions.revMailChar50) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.EmailChar50,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class SafeStringAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeStringAttribute() : base(ValidationExpressions.ValidSafeString) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidSafeString,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for phone numbers only
    /// </summary>
    public class PortNoAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public PortNoAttribute() : base(ValidationExpressions.revPortNo6) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValiedPortNo,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for phone numbers only
    /// </summary>
    public class PhoneAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public PhoneAttribute() : base(ValidationExpressions.revPhone) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Phone,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for phone numbers only
    /// </summary>
    public class Phone2Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Phone2Attribute() : base(ValidationExpressions.revPhone2) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Phone2,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule; 
        }
    }
    /// <summary>
    /// validation attribute for phone numbers only
    /// </summary>
    public class PhoneNum9Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public PhoneNum9Attribute() : base(ValidationExpressions.revPhone9) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Phone9,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for Mobile numbers only
    /// </summary>
    public class MobileAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public MobileAttribute() : base(ValidationExpressions.MobNumbersG) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Mobile,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for civil numbers only
    /// </summary>
    public class CivilNumberAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public CivilNumberAttribute() : base(ValidationExpressions.revCivilNum) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValiedCivilNum,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    
    public class NumberAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NumberAttribute() : base(ValidationExpressions.number) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class Number9Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number9Attribute() : base(ValidationExpressions.revNumber9) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly10,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class Numbers9Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Numbers9Attribute() : base(ValidationExpressions.revNumber9) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly9,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class NumberUpto30Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NumberUpto30Attribute() : base(ValidationExpressions.revNumber30) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersUpto30,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class Number10Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number10Attribute() : base(ValidationExpressions.revNumber10) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly10,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class Number15Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number15Attribute() : base(ValidationExpressions.revNumber15) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly15,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class Number5Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number5Attribute() : base(ValidationExpressions.revNumber5) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly5,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class Number4Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number4Attribute() : base(ValidationExpressions.revNumber4) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly4,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class Number2Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public Number2Attribute() : base(ValidationExpressions.revNumber2) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumbersOnly2,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class NationalIdNo : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NationalIdNo() : base(ValidationExpressions.revNationalIdNo) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NationalIdNo,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class NationalIdNo2 : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NationalIdNo2() : base(ValidationExpressions.revNationalIdNo2) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NationalIdNo2,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }



    public class NumberExpectMinusZeroAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NumberExpectMinusZeroAttribute() : base(ValidationExpressions.revNumberOnlyExpectedZeroMinus) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumberOnlyExpectMinus,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class NumberExpectZeroAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NumberExpectZeroAttribute() : base(ValidationExpressions.revNumberOnlyExpectedZero) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumberOnlyExpectMinuss,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class NumberExpectMinusZeroWithLengthAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public NumberExpectMinusZeroWithLengthAttribute() : base(ValidationExpressions.revNumberOnlyExpectedZeroMinusWithLength) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.NumberOnlyExpectMinus,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 50 characters without special characters
    /// </summary>
    public class BankAccountAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public BankAccountAttribute() : base(ValidationExpressions.BankAcoountNo) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.BankAcountNum,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 15 characters without special characters
    /// </summary>
    public class LoginNameAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public LoginNameAttribute() : base(ValidationExpressions.revLoginName) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidLoginName,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 6 characters without special characters
    /// </summary>
    public class String6Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String6Attribute() : base(ValidationExpressions.revString6) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString6,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 15 characters without special characters
    /// </summary>
    public class String15Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String15Attribute() : base(ValidationExpressions.revString15) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName50,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 20 characters without special characters
    /// </summary>
    public class String20Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String20Attribute() : base(ValidationExpressions.revString20) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName50,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 50 characters without special characters
    /// </summary>
    public class String50Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String50Attribute() : base(ValidationExpressions.revString50) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName50,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }



    public class OnlyString50Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public OnlyString50Attribute() : base(ValidationExpressions.revStringOnly50) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName50StringOnly,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }


    public class OnlyString150Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public OnlyString150Attribute() : base(ValidationExpressions.revStringOnly150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName150StringOnly,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 100 characters without special characters
    /// </summary>
    public class String100Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String100Attribute() : base(ValidationExpressions.revString100) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName100,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 150 characters without special characters
    /// </summary>
    public class String150Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String150Attribute() : base(ValidationExpressions.revString150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName150,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }



    /// <summary>
    /// validation attribute for regular expression of 750 characters without special characters
    /// </summary>
    public class String750Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String750Attribute() : base(ValidationExpressions.revString750) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularSummary750,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    ///  validation attribute for regular expression of number with min and max values
    /// </summary>
    public class RangeNumAttribute : RangeAttribute, IClientValidatable
    {
        public RangeNumAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        {
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage ?? ErrorMessageString,
                ValidationType = "range",
            };
            //rule.ValidationParameters.Add("errormessageresourcename", ErrorMessageResourceName);
            //rule.ValidationParameters.Add("errormessageresourcetype", ErrorMessageResourceType);
            rule.ValidationParameters.Add("min", Minimum);
            rule.ValidationParameters.Add("max", Maximum);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 200 characters without special characters
    /// </summary>
    public class String200Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String200Attribute() : base(ValidationExpressions.revString200) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName200,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 200 characters without special characters
    /// </summary>
    public class String6To150Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String6To150Attribute() : base(ValidationExpressions.nameRange6to150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidNameRange,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 250 characters without special characters
    /// </summary>
    public class String250Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String250Attribute() : base(ValidationExpressions.revString250) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName250,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 300 characters without special characters
    /// </summary>
    public class String300Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String300Attribute() : base(ValidationExpressions.revString300) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularSummary300,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 350 characters without special characters
    /// </summary>
    public class String350Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String350Attribute() : base(ValidationExpressions.revString350) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularSummary350,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 400 characters without special characters
    /// </summary>
    public class String400Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String400Attribute() : base(ValidationExpressions.revString400) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName400,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 450 characters without special characters
    /// </summary>
    public class String450Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String450Attribute() : base(ValidationExpressions.revString450) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularFullName450,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 500 characters without special characters
    /// </summary>
    public class String500Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public String500Attribute() : base(ValidationExpressions.revString500) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularSummary500,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 15 characters with safe special characters
    /// </summary>
    public class SafeString15Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString15Attribute() : base(ValidationExpressions.revSafeString15) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString15,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of Max characters with safe special characters
    /// </summary>
    public class SafeStringMaxAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeStringMaxAttribute() : base(ValidationExpressions.revStringmax) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidRegularNameMax,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 20 characters with safe special characters
    /// </summary>
    public class SafeString20Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString20Attribute() : base(ValidationExpressions.revSafeString20) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString20,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 20 characters with safe special characters
    /// </summary>
    public class SafeString30Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString30Attribute() : base(ValidationExpressions.revSafeString50) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString20,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    /// <summary>
    /// validation attribute for regular expression of 50 characters with safe special characters
    /// </summary>
    public class SafeString50Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString50Attribute() : base(ValidationExpressions.revSafeString50) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString50,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 100 characters with safe special characters
    /// </summary>
    public class SafeString100Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString100Attribute() : base(ValidationExpressions.revSafeString100) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString100,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 150 characters with safe special characters
    /// </summary>
    public class SafeString150Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString150Attribute() : base(ValidationExpressions.revSafeString150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString150,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 200 characters with safe special characters
    /// </summary>
    public class SafeString200Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString200Attribute() : base(ValidationExpressions.revSafeString200) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString200,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 250 characters with safe special characters
    /// </summary>
    public class SafeString250Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString250Attribute() : base(ValidationExpressions.revSafeString250) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString250,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 300 characters with safe special characters
    /// </summary>
    public class SafeString300Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString300Attribute() : base(ValidationExpressions.revSafeString300) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString300,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 350 characters with safe special characters
    /// </summary>
    public class SafeString350Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString350Attribute() : base(ValidationExpressions.revSafeString350) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString350,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 400 characters with safe special characters
    /// </summary>
    public class SafeString400Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString400Attribute() : base(ValidationExpressions.revSafeString400) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString400,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 450 characters with safe special characters
    /// </summary>
    public class SafeString450Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString450Attribute() : base(ValidationExpressions.revSafeString450) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString450,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of 500 characters with safe special characters
    /// </summary>
    public class SafeString500Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString500Attribute() : base(ValidationExpressions.revSafeString500) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString500,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of facebook url
    /// </summary>
    public class FacebookAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public FacebookAttribute() : base(ValidationExpressions.FacebookValid) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlFaceBook,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of twitter url
    /// </summary>
    public class TwitterAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public TwitterAttribute() : base(ValidationExpressions.TwitterValid) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlTwitter,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of youtube url
    /// </summary>
    public class YoutubeAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public YoutubeAttribute() : base(ValidationExpressions.ValidUrlYoutube) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlYoutube,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of youtube url
    /// </summary>
    public class YoutubeVideoAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public YoutubeVideoAttribute() : base(ValidationExpressions.ValidUrlYoutubeVideo) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlYoutubeVideo,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of GooglePlus url
    /// </summary>
    public class GooglePlusAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public GooglePlusAttribute() : base(ValidationExpressions.ValidUrlGooglePlus) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlGooglePlus,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }


    /// <summary>
    /// validation attribute for regular expression of Instagram url
    /// </summary>
    public class InstagramAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public InstagramAttribute() : base(ValidationExpressions.ValidUrlInstagram) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrlInstagram,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }




    /// <summary>
    /// validation attribute for regular expression of mail box
    /// </summary>
    public class MailBoxAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public MailBoxAttribute() : base(ValidationExpressions.revMailBoxNo) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.MailBoxNo,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of mail number(code)
    /// </summary>
    public class PostalCodeAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public PostalCodeAttribute() : base(ValidationExpressions.revPostalCode) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.PostalCode,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of url
    /// </summary>
    public class UrlAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public UrlAttribute() : base(ValidationExpressions.revUrl) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidUrl,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }


    /// <summary>
    /// validation attribute for regular expression of url
    /// </summary>
    public class UrlFullAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public UrlFullAttribute() : base(ValidationExpressions.revUrlFull) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidMapURL,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    /// <summary>
    /// validation attribute for regular expression of map Latitude and Longitiude
    /// </summary>
    public class LatitudeLongitiudeAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public LatitudeLongitiudeAttribute() : base(ValidationExpressions.revLatitudeLongitiude) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.validLatitudeLongitiude,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class PhoneSaudiArabiaAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public PhoneSaudiArabiaAttribute() : base(ValidationExpressions.revPhoneSaudiArabia) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.validPhoneSaudiArabia,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    
    }


    public class StringNoNum150Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public StringNoNum150Attribute() : base(ValidationExpressions.revStringNoNum150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeStringNoNum150,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class StringNoNum100Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public StringNoNum100Attribute() : base(ValidationExpressions.revStringNoNum100) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeStringNoNum100,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class SafeString1000Attribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public SafeString1000Attribute() : base(ValidationExpressions.revSafeString1000) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.SafeString1000,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class StringAllowSpectialCharacter150 : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public StringAllowSpectialCharacter150() : base(ValidationExpressions.ValidSafeString150) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.Length150,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class ValidDecimalNum : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public ValidDecimalNum() : base(ValidationExpressions.ValidDecimalNum) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidDecimalNum,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    public class ValidDecimal15Num : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public ValidDecimal15Num() : base(ValidationExpressions.ValidDecimal15Num) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidDecimal15Num,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
    public class ValidDecimalNum3 : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public ValidDecimalNum3() : base(ValidationExpressions.ValidDecimalNum3) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidDecimalNum3,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }
   
    public class ValidDecimalNumWithoutZero : System.ComponentModel.DataAnnotations.RegularExpressionAttribute, IClientValidatable
    {
        public ValidDecimalNumWithoutZero() : base(ValidationExpressions.ValidDecimalNumWithoutZero) { }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ValidationMessages.ValidDecimalNumWithoutZero,
                ValidationType = "regex",
            };
            rule.ValidationParameters.Add("pattern", Pattern);

            yield return rule;
        }
    }

    ///////////////////////////////////
    [AttributeUsage(AttributeTargets.Property)]
    public class GenericRemote : System.Web.Mvc.RemoteAttribute//, IClientValidatable
    {
        public GenericRemote(string Attr)
            : base("CheckDuplicate", HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString())
        {
         //   base.HttpMethod = "Post";
            //base.GetClientValidationRules();

            HttpContext.Current.Session["Attr"] = Attr;
        }
        //private GenericRemote(string routeName)
        //    : base(routeName)
        //{

        //}
        private GenericRemote(string action, string controller,string Attr)
            : base(action, controller)
        {

            HttpContext.Current.Session["Attr"] = Attr;
        }
        private GenericRemote(string action, string controller, string areaName,string Attr)
            : base(action, controller, areaName)
        {
            HttpContext.Current.Session["Attr"] = Attr;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
         //   var fields = validationContext.ObjectType.GetProperty(this.AdditionalFields);
          //  var fieldsValue = (string)fields.GetValue(validationContext.ObjectInstance, null);
            return base.IsValid(value,validationContext);
        }


        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{

        //    var rule = new ModelClientValidationRule
        //    {
        //        ErrorMessage = ValidationMessages.SafeStringNoNum150,
        //        ValidationType = "Remote",
        //    };
        //   // rule.ValidationParameters.Add("pattern", Pattern);

        //    yield return rule;
        //}
    }
    ///////////////////////////////////

}