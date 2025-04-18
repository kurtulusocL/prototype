﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;

namespace DataAccess.Concrete.EntityFramework
{
    public class ImageDal : EntityRepositoryBase<Image, ApplicationDbContext>, IImageDal
    {
        public ImageDal(ApplicationDbContext context) : base(context)
        {
        }
    }
}
