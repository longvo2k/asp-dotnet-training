namespace StudyDotnet.Dtos;

public sealed record DeviceDto(Guid Id, string Name, string Supplier, bool IsOnline, string CompanyName);
