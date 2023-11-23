using MediatR;
using Microsoft.AspNetCore.Identity;
using QuizWiz.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWix.Application.Auth.Commands
{
    public class AddUserToRoleCommand : IRequest
    {
        public string Email { get; set;} = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }


    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand>
    {
        private readonly UserManager<User> _userManager;

        public AddUserToRoleCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, request.RoleName);
            }
        }
    }
}
