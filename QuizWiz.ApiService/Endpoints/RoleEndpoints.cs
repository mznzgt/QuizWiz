using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using QuizWix.Application.Auth.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuizWiz.ApiService.Endpoints
{
    public static class RoleEndpoints
    {
        public static void MapUserRoleEndpoints(this WebApplication app)
        {
            app.MapPost("/user/role", async (string email, string roleName, IMediator mediator) =>
            {
                var addUserToRoleCommand = new AddUserToRoleCommand
                {
                    Email = email,
                    RoleName = roleName
                };

                await mediator.Send(addUserToRoleCommand);
            }).RequireAuthorization();
        }

    }
}
