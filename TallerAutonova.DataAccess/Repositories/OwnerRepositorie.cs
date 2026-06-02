using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TallerAutonova.DataAccess.Context;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Repositories;

namespace TallerAutonova.DataAccess.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(TallerDbContext context) : base(context)
        {
        }
    }
}