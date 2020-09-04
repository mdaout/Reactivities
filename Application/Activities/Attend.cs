using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Application.Activities
{
    public class Attend
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            // Properites go here 
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //  var activity = new Activity
                // {   Id = request.Id,
                //      Venue = request.Venue
                // };
                //  _context.Activities.Add(activity);
                // Handler goes Logic here    
                var actvivity = await _context.Activities.FindAsync(request.Id);

                if (actvivity == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Activity = "Could not be Found" });
                }

                var user = await _context.Users.SingleOrDefaultAsync(x =>
                     x.UserName == _userAccessor.GetCurrentUsername());


                var attendee = await _context.UserActivities.SingleOrDefaultAsync(x =>
                      x.ActivityId == actvivity.Id && x.AppUserId == user.Id);

                if (attendee  != null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Attendance= "Already attending this activity" });
                }
                
                var Attendance = new UserActivity {
                 Activity = actvivity,
                 AppUser = user,
                 IsHost= false,
                 DateJoined = DateTime.Now
                };
                
                 _context.UserActivities.Add(Attendance);

                var success = await _context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Problem adding record");
            }
        }
    }
}