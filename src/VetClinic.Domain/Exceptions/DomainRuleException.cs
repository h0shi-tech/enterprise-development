namespace VetClinic.Domain.Exceptions;

/// <summary>
/// Операция нарушает правило предметной области
/// </summary>
public sealed class DomainRuleException(string message) : Exception(message);
