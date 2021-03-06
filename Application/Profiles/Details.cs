using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Details
    {

        public class Query : IRequest<Profile>
        {

            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                // Handler Logic  Goes Here      
                var user = await _context.Users.SingleOrDefaultAsync(i => i.UserName == request.UserName);
                return new Profile
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Image = user.Photos.FirstOrDefault(i => i.IsMain)?.Uri,
                    Photos = user.Photos,
                    Bio = user.Bio

                };
            }

        }
    }
}