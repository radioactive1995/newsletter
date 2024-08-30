﻿using Application.Users;

namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    public UserInformationDto? GetUserInformation();
    public record UserInformationDto(string? UserId, string? UserName, string? Email);

    public string? GetIpAddress();
}

