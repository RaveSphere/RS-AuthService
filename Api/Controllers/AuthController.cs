using Api.Mappers;
using Api.RequestModels;
using Api.ResponseModels;
using Api.ViewModels;
using Application.Interfaces;
using Core.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IValidator<CreateUserRequest> CreateUserValidator, IValidator<LoginRequest> LoginValidator, HttpClient httpClient, IHashingService hashingService, ITokenService tokenService, IConfiguration configuration) : ControllerBase
    {
        private string? _userServiceBaseUrl = configuration["UserServiceBaseUrl"];

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResult = await CreateUserValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            HashingModel hashingModel = await hashingService.HashAsync(request.Username, request.Password, Guid.NewGuid());

            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_userServiceBaseUrl}/api/Users/create-user", UserMapper.Map(hashingModel), cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                CreateUserResponse? createUserResponse = await response.Content.ReadFromJsonAsync<CreateUserResponse>(cancellationToken);
                if (createUserResponse != null)
                {
                    return Ok(UserMapper.Map(createUserResponse));
                }
            }

            return BadRequest("Could not create user");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResult = await LoginValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            HttpResponseMessage saltResponse = await httpClient.PostAsJsonAsync($"{_userServiceBaseUrl}/api/Users/get-user-salt", UserMapper.SaltMap(request), cancellationToken);

            if (saltResponse.IsSuccessStatusCode)
            {
                GetUserSaltResponse? getUserSaltResponse = await saltResponse.Content.ReadFromJsonAsync<GetUserSaltResponse>(cancellationToken);

                if (getUserSaltResponse != null)
                {
                    HashingModel hashingModel = await hashingService.HashAsync(request.Username, request.Password, getUserSaltResponse.Salt);

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_userServiceBaseUrl}/api/Users/validate-credentials", LoginMapper.Map(hashingModel), cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        GetUserResponse? getUserResponse = await response.Content.ReadFromJsonAsync<GetUserResponse>(cancellationToken);

                        if (getUserResponse != null)
                        {
                            string token = tokenService.GenerateToken(LoginMapper.Map(getUserResponse));

                            return Ok(new TokenVM(token));
                        }
                    }
                }
            }

            return BadRequest("Username or password is wrong");
        }
    }
}
