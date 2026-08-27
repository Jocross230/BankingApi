using BankingApi.DTOs.Auth;
using BankingApi.Helpers;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        JwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Step 1: Normalize email
        var email = request.Email.Trim().ToLowerInvariant();

        // Step 2: Check if email already exists
        var existingUser =
            await _userRepository.GetByEmailAsync(email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        // Step 3: Hash password
        var passwordHash =
            BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Step 4: Create user model
        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = passwordHash
        };

        // Step 5: Save user and get generated ID
        var userId = await _userRepository.CreateAsync(user);

        user.Id = userId;

        // Step 6: Generate account number
        var accountNumber = GenerateAccountNumber(userId);

        // Step 7: Create account
        var account = new Account
        {
            UserId = userId,
            AccountNumber = accountNumber,
            Balance = 0
        };

        await _accountRepository.CreateAsync(account);

        // Step 8: Generate JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Step 9: Return safe response
        return new AuthResponse
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Step 1: Normalize email
        var email = request.Email.Trim().ToLowerInvariant();

        // Step 2: Find user
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        // Step 3: Verify password
        var isPasswordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        // Step 4: Generate JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Step 5: Return response
        return new AuthResponse
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    private static string GenerateAccountNumber(int userId)
    {
        return $"10{userId:D8}";
    }
}