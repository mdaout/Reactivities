using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class List
    {
     //   public class Query : IRequest<List<Activity>> { } before eager loading
        public class Query : IRequest<List<ActivityDto>> { }


       // public class Handler : IRequestHandler<Query, List<Activity>> before eager loading
        public class Handler : IRequestHandler<Query, List<ActivityDto>>
        {
             private readonly DataContext _context;
             
             private readonly IMapper _mapper;
            // public Handler(DataContext context) before eager loading
            
            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
  

         //   public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken) before eager loading
            public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                   // need Microsoft.EntityFrameworkCore.Proxies for Lazy Loading

                   //Eager Loading can be mixed with Lazy
                //  var activities = await _context.Activities.
                //  Include(x => x.UserActivities).
                //  ThenInclude(x => x.AppUser).ToListAsync();   

                // // // var activities = await _context.UserActivities.
                // // //  Include(x => x.Activity).
                // // //  Include(x => x.AppUser).ToListAsync();   

               // Lazy 
                 var activities = await _context.Activities.ToListAsync();  

               //   var activities = await _context.Activities.Include.     before eager loading

              // return activities; before eager loading
             return _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            // // // return new List<ActivityDto>();
            }
        }
    }
}