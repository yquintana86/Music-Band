using Application.Abstractions.Messaging;

namespace Application.Musicians.Query.SearchDomesticSeniorMusicians;

public sealed record SearchDomesticSeniorMusiciansQuery(int age) : IQuery<int>;
