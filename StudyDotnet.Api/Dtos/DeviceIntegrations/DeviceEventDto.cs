namespace StudyDotnet.Dtos;

public sealed record DeviceEventDto(
    string DeviceSerialNumber,
    string EmployeeCode,
    DateTime EventTime,
    string EventType);
