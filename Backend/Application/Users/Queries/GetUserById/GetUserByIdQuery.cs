using Application.Abstractions.Messaging;
using Shared.Models.Authentication;

namespace Application.UserApp.Queries.SearchUserById;

public sealed record GetUserByIdQuery(int Id) : IQuery<UserResponse>;
