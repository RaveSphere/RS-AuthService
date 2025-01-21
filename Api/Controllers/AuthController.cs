using Api.Mappers;
using Api.RequestModels;
using Api.ResponseModels;
using Application.Interfaces;
using Core.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IValidator<CreateUserRequest> validator, HttpClient httpClient, IHashingService hashingService, IConfiguration configuration) : ControllerBase
    {
        private string? _userServiceBaseUrl;

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            HashingModel hashingModel = await hashingService.HashAsync(request.Username, request.Password, Guid.NewGuid());

            _userServiceBaseUrl = configuration["UserServiceBaseUrl"];
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{_userServiceBaseUrl}/api/Users", UserMapper.Map(hashingModel), cancellationToken);

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

        //[HttpGet]
        //public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken = default)
        //{
        //    return Ok();
        //}
    }
}
