using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Contracts;

public record RegistrationResponse(
    [Required] string Email,
    [Required] string UserName
);