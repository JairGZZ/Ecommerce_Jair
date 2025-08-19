namespace Ecommerce_Jair.Server.DTOs.Auth;
public record UserTokenDTO
(
     int UserId,
     string FirstName ,
     string LastName ,
     string Email 
);