using Application.Abstractions.Authentication;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWorks;
using Application.DTOs;
using Domain.Authentication;
using Domain.Core.Errors;
using Domain.Core.Results;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public interface IAuthenticationService
{
    Task<Result<AccessToken>> CreateTokenAsync(LoginDto loginDto);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager, 
        IJwtProvider jwtProvider, 
        IRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccessToken>> CreateTokenAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Result.Failure<AccessToken>(DomainErrors.Authentication.InvalidEmailOrPassword);
        }
        var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!checkPassword)
        {
            return Result.Failure<AccessToken>(DomainErrors.Authentication.InvalidEmailOrPassword);
        }
        var token = await _jwtProvider.CreateToken(user);

        var userRefreshToken = await _refreshTokenRepository.GetAsync(x => x.UserId == user.Id);
        if (userRefreshToken == null)
        {
            await _refreshTokenRepository.CreateAsync(RefreshToken.Create(user.Id, token.RefreshToken, token.RefreshTokenExpiration));
        }
        else
        {
            userRefreshToken.UpdateToken(token.RefreshToken, token.RefreshTokenExpiration);
        }

        await _unitOfWork.SaveChangesAsync();
        return Result.Success(token);

    }
}
