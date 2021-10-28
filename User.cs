using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// User entity
/// </summary>

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(128)]
    public string UserName { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string FullName { get; set; }
    
    public DateTime Birthday { get; set; }

    [MaxLength(1000)]
    public string Photo { get; set; }

    [Required]
    public DateTime DateTimeAdd { get; set; }
}