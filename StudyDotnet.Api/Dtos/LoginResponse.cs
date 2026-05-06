namespace StudyDotnet.Api.Dtos;

public sealed record LoginResponse(string AccessToken, string TokenType, string Tenant);
