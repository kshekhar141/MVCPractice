﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyShoppingCart.Models.Data
{
    public class Db:DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}