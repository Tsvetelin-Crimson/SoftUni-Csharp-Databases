using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SupplierModel, Supplier>();
            this.CreateMap<PartModel, Part>();
            this.CreateMap<CustomerModel, Customer>();
            this.CreateMap<SaleModel, Sale>();
        }
    }
}
