using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicStore.Services.Implementations;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IOptions<AppSettings> _options;
    private readonly UserManager<MusicStoreUserIdentity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ICustomerRepository _customerRepository;

    public UserService(ILogger<UserService> logger, 
        IOptions<AppSettings> options, 
        UserManager<MusicStoreUserIdentity> userManager, 
        RoleManager<IdentityRole> roleManager,
        ICustomerRepository customerRepository)
    {
        _logger = logger;
        _options = options;
        _userManager = userManager;
        _roleManager = roleManager;
        _customerRepository = customerRepository;
    }
    
    public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
    {
        var response = new LoginDtoResponse();

        try
        {
            var identity = await _userManager.FindByEmailAsync(request.UserName);

            if (identity == null)
            {
                throw new ApplicationException("Usuario no existe");
            }

            var result = await _userManager.CheckPasswordAsync(identity, request.Password);
            if (!result)
                throw new ApplicationException("Usuario o clave incorrectos");

            var expiredDate = DateTime.Now.AddDays(1);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Expiration, expiredDate.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            // Creacion de Token JWT
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Jwt.SecretKey));

            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(_options.Value.Jwt.Issuer, 
                _options.Value.Jwt.Audience, 
                claims, 
                DateTime.Now, 
                expiredDate);

            var token = new JwtSecurityToken(header, payload);

            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
            
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LoginAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }

    public async Task<BaseResponseGeneric<string>> RegisterAsync(RegisterDtoRequest request)
    {
        var response = new BaseResponseGeneric<string>();

        try
        {
            var user = new MusicStoreUserIdentity
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age,
                DocumentNumber = request.DocumentNumber,
                DocumentType = request.DocumentType,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var userIdentity = await _userManager.FindByNameAsync(user.UserName);

                if (userIdentity != null)
                {
                    if (!await _roleManager.RoleExistsAsync(request.Role))
                        await _roleManager.CreateAsync(new IdentityRole(request.Role));

                    await _userManager.AddToRoleAsync(userIdentity, request.Role);

                    // Creamos el objeto Customer con el registro del usuario.
                    var customer = new Customer
                    {
                        FullName = $"{request.FirstName} {request.LastName}",
                        Email = request.Email,
                    };

                    await _customerRepository.AddAsync(customer);

                    response.Success = true;
                    response.Data = user.Id;
                }
            }
            else
            {
                response.Success = false;
                var sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.AppendLine(error.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Length = 0;
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RegisterAsync");
            response.Success = false;
            response.ErrorMessage = "Error in RegisterAsync";
        }

        return response;
    }
}