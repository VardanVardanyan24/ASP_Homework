using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
public class DateInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dt)
        {
            return dt < DateTime.Today;
        }
        return true;
    }
}