namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Common.Mvc.Routing;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Identity;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    using static GamaEdtech.Common.Core.Constants;

    [Common.DataAnnotation.Area(nameof(Role.Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    [Display(Name = "Users")]
    public class IdentitiesController(Lazy<ILogger<IdentitiesController>> logger, Lazy<IIdentityService> identityService, Lazy<IEndpointDataSource> endpointDataSource, Lazy<IStringLocalizer<IdentitiesController>> localizer)
        : LocalizableApiControllerBase<IdentitiesController>(logger, localizer)
    {
        [HttpGet, Produces(typeof(ApiResponse<ListDataSource<UserListResponseViewModel>>))]
        [Display(Name = "Users List")]
        public async Task<IActionResult<ListDataSource<UserListResponseViewModel>>> GetUsers([NotNull, FromQuery] ConsumersRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.GetUsersAsync(new ListRequestDto<ApplicationUser> { PagingDto = request.PagingDto });
                return Ok<ListDataSource<UserListResponseViewModel>>(new(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new UserListResponseViewModel
                        {
                            Id = t.Id,
                            Username = t.UserName,
                            Email = t.Email,
                            PhoneNumber = t.PhoneNumber,
                            Enabled = t.Enabled,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<UserListResponseViewModel>>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpGet("{userId:int}"), Produces(typeof(ApiResponse<UserResponseViewModel>))]
        [Display(Name = "User Details")]
        public async Task<IActionResult<UserResponseViewModel>> Get([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.GetUserAsync(new IdEqualsSpecification<ApplicationUser, int>(userId));
                return Ok<UserResponseViewModel>(new(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Username = result.Data.UserName,
                        Email = result.Data.Email,
                        PhoneNumber = result.Data.PhoneNumber,
                        Enabled = result.Data.Enabled,
                        RegistrationDate = result.Data.RegistrationDate,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<UserResponseViewModel>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPost, Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Create User")]
        public async Task<IActionResult> Create([NotNull] CreateUserRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.CreateUserAsync(new CreateUserRequestDto
                {
                    Username = request.Username,
                    Password = request.Password,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Avatar = await request.Avatar.ConvertImageToBase64Async(),
                });
                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPut("{userId:int}"), Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Edit User")]
        public async Task<IActionResult<Void>> Update([FromRoute] int userId, [NotNull, FromBody] EditUserRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.UpdateUserAsync(new UpdateUserRequestDto
                {
                    Id = userId,
                    Username = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Avatar = await request.Avatar.ConvertImageToBase64Async(),
                });
                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpDelete("{userId:int}"), Produces(typeof(ApiResponse<bool>))]
        [Display(Name = "Remove User")]
        public async Task<IActionResult> Remove([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.RemoveUserAsync(new IdEqualsSpecification<ApplicationUser, int>(userId));
                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPatch("{userId:int}/toggle"), Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Enable/Disable User")]
        public async Task<IActionResult> Toggle([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.ToggleUserAsync(new IdEqualsSpecification<ApplicationUser, int>(userId));
                return Ok<Void>(new(result.Errors)
                {
                    Data = new()
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpGet("{userId:int}/token"), Produces(typeof(ApiResponse<GetTokenResponseViewModel>))]
        [Display(Name = "View User Token")]
        public async Task<IActionResult> GetUserToken([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.GetUserTokenAsync(new GetUserTokenRequestDto
                {
                    UserId = userId,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok<GetTokenResponseViewModel>(new(result.Errors)
                {
                    Data = new GetTokenResponseViewModel
                    {
                        Token = result.Data,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<GetTokenResponseViewModel>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPost("{userId:int}/token"), Produces(typeof(ApiResponse<GenerateTokenResponseViewModel>))]
        [Display(Name = "Generate User Token")]
        public async Task<IActionResult<GenerateTokenResponseViewModel>> GenerateToken([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.GenerateUserTokenAsync(new GenerateUserTokenRequestDto
                {
                    UserId = userId,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok<GenerateTokenResponseViewModel>(new(result.Errors)
                {
                    Data = new GenerateTokenResponseViewModel
                    {
                        Token = result.Data?.Token,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<GenerateTokenResponseViewModel>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpDelete("{userId:int}/token"), Produces(typeof(ApiResponse<bool>))]
        [Display(Name = "Remove User Token")]
        public async Task<IActionResult<bool>> RemoveToken([FromRoute] int userId)
        {
            try
            {
                var result = await identityService.Value.RemoveUserTokenAsync(new RemoveUserTokenRequestDto
                {
                    UserId = userId,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPut("{userId:int}/reset-password"), Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Reset User Password")]
        public async Task<IActionResult<Void>> ResetPassword([FromRoute] int userId, [NotNull] ResetPasswordRequestViewModel request)
        {
            try
            {
                var userResult = await identityService.Value.GetUserAsync(new IdEqualsSpecification<ApplicationUser, int>(userId));
                if (userResult.OperationResult is not OperationResult.Succeeded)
                {
                    return Ok<Void>(new(userResult.Errors));
                }
                var resetPasswordResult = await identityService.Value.ResetPasswordAsync(new ResetPasswordRequestDto
                {
                    UserId = userId,
                    NewPassword = request.NewPassword,
                });
                return Ok<Void>(new(resetPasswordResult.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpGet("{userId:int}/permissions"), Produces(typeof(ApiResponse<UserPermissionsResponseViewModel>))]
        [Display(Name = "View User Permissions")]
        public async Task<IActionResult<UserPermissionsResponseViewModel>> Permissions([FromRoute] int userId)
        {
            try
            {
                var endpoints = endpointDataSource.Value.GetEndpoints();
                if (endpoints is null)
                {
                    return Ok(new ApiResponse<UserPermissionsResponseViewModel>());
                }

                var result = await identityService.Value.GetUserPermissionsAsync(new UserPermissionsRequestDto { UserId = userId });

                var areas = endpoints.Where(t => t.Controller.HasValue).GroupBy(t => t.Area);
                List<PermissionsResponseViewModel> items = new(areas.Count());
                foreach (var area in areas)
                {
                    var controllers = area.GroupBy(t => t.Controller);
                    var areaItem = new PermissionsResponseViewModel
                    {
                        Text = area.Key?.Value ?? Localizer.Value["Root"],
                        Items = controllers.Select(t => new PermissionsResponseViewModel
                        {
                            Text = t.Key?.Value,
                            Items = t.Select(c => new PermissionsResponseViewModel
                            {
                                Text = c.Action?.Value,
                                Value = c.EndpointName,
                                HasPermission = result.Data?.Permissions?.Contains(c.EndpointName) is true,
                            }),
                        }),
                    };

                    items.Add(areaItem);
                }

                return Ok<UserPermissionsResponseViewModel>(new(result.Errors)
                {
                    Data = new UserPermissionsResponseViewModel
                    {
                        Permissions = items,
                        Roles = result.Data?.Roles,
                        SystemClaims = result.Data?.SystemClaims,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<UserPermissionsResponseViewModel>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }

        [HttpPut("{userId:int}/permissions"), Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Edit User Permissions")]
        public async Task<IActionResult<Void>> ManageUserPermissions([FromRoute] int userId, [NotNull] ManageUserPermissionsRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.UpdateUserPermissionsAsync(new UpdateUserPermissionsRequestDto
                {
                    UserId = userId,
                    Permissions = request.Permissions ?? [],
                    Roles = request.Roles,
                    SystemClaims = request.SystemClaims,
                });

                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }
    }
}
