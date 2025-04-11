﻿using System.Text.RegularExpressions;

namespace Domain.Common.Helpers;

public class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
        
        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        var regex = new Regex(emailPattern);

        return regex.IsMatch(email);
    }
}