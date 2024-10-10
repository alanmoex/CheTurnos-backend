using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ImageUrlRepository: RepositoryBase<ImageUrl>, IImageUrlRepository
    {
        public ImageUrlRepository(AppDbContext context) : base(context)
        {

        }
    }
}
