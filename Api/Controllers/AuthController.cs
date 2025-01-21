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
    public class AuthController(IValidator<CreateUserRequest> validator, HttpClient httpClient, IHashingService hashingService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            HashingModel hashingModel = await hashingService.Hash(request.Username, request.Password, Guid.NewGuid());

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7221/api/Users", hashingModel, cancellationToken);

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
