using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Activities
{
    public class Details
    {

        // public class Query : IRequest<Activity>  //before eager loading
        public class Query : IRequest<ActivityDto>
        {

            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, ActivityDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }
            public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                //  throw new Exception(" Ain't Goona Happen");

                // need Microsoft.EntityFrameworkCore.Proxies for Lazy Loading

                // Eager Loading
                // var activity = await _context.Activities.
                // Include(x => x.UserActivities).
                // ThenInclude(x => x.AppUser).SingleOrDefaultAsync(x => x.Id == request.Id);

                //Lazy Loading 
                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Not found" });

                var ActivityToReturn = _mapper.Map<Activity, ActivityDto>(activity);

                return ActivityToReturn;
            }
        }
    }
}