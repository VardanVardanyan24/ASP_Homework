﻿using System;
using System.ComponentModel.DataAnnotations;

public class EditEntityDto
{
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
    public string Username { get; set; }

    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string Email { get; set; }

    [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [CustomPasswordValidation("Username")]
    public string Password { get; set; }

    [DataType(DataType.Date)]
    [DateInPast(ErrorMessage = "Date of birth must be in the past.")]
    public DateTime DateOfBirth { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
    public int Quantity { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a decimal value")]
    public string Price { get; set; }

    [Range(0, 49, ErrorMessage = "Amount must be less than 50")]
    public int Amount { get; set; }
}