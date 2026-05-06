namespace StudyDotnet.Api.Dtos;

public sealed record DeviceDto(Guid Id, string Name, string Supplier, bool IsOnline, string CompanyName);
