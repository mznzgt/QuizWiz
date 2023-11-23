using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizWix.Application.Auth.Commands
{
    public class AddRolesCommand : IRequest
    {
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }

    public class AddRolesCommandHandler : IRequestHandler<AddRolesCommand>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRolesCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Handle(AddRolesCommand request, CancellationToken cancellationToken)
        {
            foreach (var role in request.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }


}
