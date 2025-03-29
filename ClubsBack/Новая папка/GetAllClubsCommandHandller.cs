using ClubsBack.Entities;
using ClubsBack.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ClubsBack.Новая_папка
{
    public class GetAllClubsCommandHandller : IRequestHandler<GetAllClubsCommand, List<Clubs>>
    {
        private IClubs _repository;
        public GetAllClubsCommandHandller(IClubs usersRepository)
        {
            _repository = usersRepository;
        }
        
        public async Task<List<Clubs>> Handle(GetAllClubsCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_repository.Get());
        }
    }
}
