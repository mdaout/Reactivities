using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Photo>    // add photo as return type
        {
            // public Guid Id { get; set; }
            public IFormFile File { get; set; }  // Only one paramter
                                                 // Properites go here 
        }
        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }
            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {

                var photoUploadResult = _photoAccessor.AddPhoto(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName ==
                _userAccessor.GetCurrentUsername());

                var photo = new Photo
                {
                    Uri = photoUploadResult.Uri,
                    Id = photoUploadResult.PublicId
                };
                if (!user.Photos.Any(x => x.IsMain))
                {
                    photo.IsMain = true;
                }
                user.Photos.Add(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return photo;
                throw new Exception("Problem adding record");
            }
        }
    }
}