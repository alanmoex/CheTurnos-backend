﻿using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IServiceRepository : IRepositoryBase<Service>
    {
        public List<Service> GetAllByShopId(int shopId);

    }
}
