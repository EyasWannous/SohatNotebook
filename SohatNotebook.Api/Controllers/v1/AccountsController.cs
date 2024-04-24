using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SohatNotebook.Authentication.Configuration;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.DbSet;
using SohatNoteBook.Authentication.Models.DTOs.Generic;
using SohatNoteBook.Authentication.Models.DTOs.Incoming;
using SohatNoteBook.Authentication.Models.DTOs.Outcoming;

namespace SohatNotebook.Api.Controllers.v1;

public class AccountsController(IUnitOfWork unitOfWork,
    UserManager<IdentityUser> userManager, IOptionsMonitor<JWTConfig> options,
    TokenValidationParameters tokenValidationParameters, IMapper mapper)
    : BaseController(unitOfWork, userManager, mapper)
{
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    private readonly JWTConfig _options = options.CurrentValue;


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO userRegistrationRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new UserRegistartionResponseDTO
            {
                Seccess = false,
                Errors = ["Invalid payload"]
            });
        }

        var user = await _userManager.FindByEmailAsync(userRegistrationRequestDTO.Email);
        if (user is not null)
        {
            return BadRequest(new UserRegistartionResponseDTO
            {
                Seccess = false,
                Errors = ["Email is already taken"]
            });
        }

        var newUser = new IdentityUser
        {
            Email = userRegistrationRequestDTO.Email,
            UserName = userRegistrationRequestDTO.Email,
            EmailConfirmed = true, // Todo build email functionality to make user confirm his email
        };

        var isCreated = await _userManager.CreateAsync(newUser, userRegistrationRequestDTO.Password);
        if (!isCreated.Succeeded)
        {
            return BadRequest(new UserRegistartionResponseDTO
            {
                Seccess = false,
                Errors = isCreated.Errors.Select(error => error.Description).ToList(),
            });
        }

        var _user = new User
        {
            IdentityId = new Guid(newUser.Id),
            FirstName = userRegistrationRequestDTO.FirstName,
            LastName = userRegistrationRequestDTO.LastName,
            Email = userRegistrationRequestDTO.Email,
            Phone = "userRegistrationRequestDTO.Phone",
            DateOfBirth = DateTime.UtcNow, // Convert.ToDateTime(userRegistrationRequestDTO.DateOfBirth),
            Country = "userRegistrationRequestDTO.Country",
            Address = "Glasgow, UK",
            MobileNumber = "123123213",
            Sex = "Male",
        };

        await _unitOfWork.UsersRepository.AddAysnc(_user);

        await _unitOfWork.CompleteAsync();

        var token = await GenerateJWTToken(newUser);

        return Ok(new UserRegistartionResponseDTO
        {
            Seccess = true,
            Token = token.JWTToken,
            RefreshToken = token.RefreshToken,
        });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO userLoginRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = false,
                Errors = ["Invalid payload"]
            });
        }

        var user = await _userManager.FindByEmailAsync(userLoginRequestDTO.Email);
        if (user is null)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = false,
                Errors = ["Invalid authentication request"]
            });
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, userLoginRequestDTO.Password);
        if (!isPasswordCorrect)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = false,
                Errors = ["Invalid authentication request"]
            });
        }

        var token = await GenerateJWTToken(user);

        return Ok(new UserLoginResponseDTO
        {
            Seccess = true,
            Token = token.JWTToken,
            RefreshToken = token.RefreshToken,
        });
    }


    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequestDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = false,
                Errors = ["Invalid payload"]
            });
        }

        var result = await VerfiyToken(tokenRequestDTO);
        if (result is null)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = false,
                Errors = ["Token validation falid"]
            });
        }

        if (!result.Seccess)
        {
            return BadRequest(new UserLoginResponseDTO
            {
                Seccess = result.Seccess,
                Errors = result.Errors,
            });
        }

        return Ok(result);
    }

    private async Task<AuthResult?> VerfiyToken(TokenRequestDTO tokenRequestDTO)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler
                .ValidateToken(tokenRequestDTO.Token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken)
                return null;

            var result = jwtSecurityToken.Header.Alg == "HS256";
            // .Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase);
            if (!result)
                return null;

            var didParsed = long.TryParse
            (
                principal.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp)?.Value,
                out long utcExpiryDate
            );
            if (!didParsed)
                return null;

            var expDate = await UnixTimeStampToDateTimeAsync(utcExpiryDate);
            if (expDate > DateTime.UtcNow)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["JWT Token has not expired"],
                };
            }

            var refreshToken = await _unitOfWork.RefreshTokens
                .GetByRefreshTokenAsync(tokenRequestDTO.RefreshToken);
            if (refreshToken is null)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Invalid Refresh Token"],
                };
            }

            if (refreshToken.ExpireDate < DateTime.UtcNow)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Refresh Token has expired, please login again"],
                };
            }


            if (refreshToken.IsUsed)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Refresh Token has been used, it cannot be reused"],
                };
            }

            if (refreshToken.IsRevoked)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Refresh Token has been revoked, it cannot be reused"],
                };
            }

            var jtiId = principal.Claims.SingleOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jtiId is null)
                return null;

            if (refreshToken.JWTId != jtiId)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Refresh Token refrence dose not match the jwt token"],
                };
            }

            refreshToken.IsUsed = true;

            var updateResult = await _unitOfWork.RefreshTokens.MakeRefreshTokenAsUsedAsync(refreshToken);
            if (!updateResult)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Error processing request"],
                };
            }

            await _unitOfWork.CompleteAsync();

            var dbUser = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (dbUser is null)
            {
                return new AuthResult
                {
                    Seccess = false,
                    Errors = ["Error processing request"],
                };
            }

            var newTokens = await GenerateJWTToken(dbUser);

            return new AuthResult
            {
                Seccess = true,
                Token = newTokens.JWTToken,
                RefreshToken = newTokens.RefreshToken,
            };

        }
        catch (Exception)
        {
            // Todo add better error handling
            // Todo add logger
            return null;
        }
    }

    private async Task<DateTime> UnixTimeStampToDateTimeAsync(long unixDate)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, 0, DateTimeKind.Utc);

        dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();

        return await Task.FromResult(dateTime);
    }

    private async Task<TokenDataDTO> GenerateJWTToken(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var signingKey = Encoding.ASCII.GetBytes(_options.SigningKey);

        List<Claim> claims =
        [
            new("Id",user.Id),
            new(ClaimTypes.NameIdentifier,user.Id),
            new(JwtRegisteredClaimNames.Sub, user.Email!), // unique id
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // used by refresh token
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_options.ExpireTime), // Todo Update the expiration time to (5 min)
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwtToken = tokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            AddedDate = DateTime.UtcNow,
            Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
            UserId = user.Id,
            IsRevoked = false,
            IsUsed = false,
            JWTId = token.Id,
            ExpireDate = DateTime.UtcNow.AddMonths(6),
        };

        await _unitOfWork.RefreshTokens.AddAysnc(refreshToken);
        await _unitOfWork.CompleteAsync();

        var tokenData = new TokenDataDTO
        {
            JWTToken = jwtToken,
            RefreshToken = refreshToken.Token,
        };

        return tokenData;
    }


    private string RandomStringGenerator(int lenght)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        return new string(Enumerable.Repeat(chars, lenght).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}