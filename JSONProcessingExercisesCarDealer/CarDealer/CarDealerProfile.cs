﻿namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Suppliers
            this.CreateMap<ImportSupplierDto, Supplier>();

            //Part
            this.CreateMap<ImportPartDto, Part>();

            //Car
            this.CreateMap<ImportCarDto, Car>();

            //Customer
            this.CreateMap<ImportCustomerDto, Customer>();

            //Sale
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
