using AutoMapper;
using CarDealer.Data;
using CarDealer.DataTransferObjects.Input;
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using CarDealer.Models;
using CarDealer.XmlHelper;
using CarDealer.DataTransferObjects.Output;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();
            //context.Database.EnsureCreated();
            //var suppliersXML = File.ReadAllText("Datasets/suppliers.xml");
            //var partsXML = File.ReadAllText("Datasets/parts.xml");
            //var carsXML = File.ReadAllText("Datasets/cars.xml");
            //var customersXML = File.ReadAllText("Datasets/customers.xml");
            //var salesXML = File.ReadAllText("Datasets/sales.xml");


            //ImportSuppliers(context, suppliersXML);
            //ImportParts(context, partsXML);
            //ImportCars(context, carsXML)
            //ImportCustomers(context, customersXML)
            //ImportSales(context, salesXML)
            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new SalesWithDiscountOutputModel
                {
                    Car = new MiniCarOutputModel
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount = x.Discount,
                    CustomerName = x.Customer.Name,
                    Price = x.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = x.Car.PartCars.Sum(pc => pc.Part.Price) - x.Car.PartCars.Sum(pc => pc.Part.Price) * x.Discount / 100m
                })
                .ToList();


            var result = XmlConverter.Serialize(sales, "sales");

            return result;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var suppliers = context.Customers
                    .Where(x => x.Sales.Count() > 0)
                .Select(x => new CustomerSalesOuputModel
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales //.Sum(s => s.Car.PartCars.SelectMany(pc => pc.Part.Price)) this doesnt work locally for some reason but in judge it does
                                .Select(x => x.Car).SelectMany(x => x.PartCars).Sum(x => x.Part.Price),
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToList();


            var result = XmlConverter.Serialize(suppliers, "customers");

            return result;

        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {

            var cars = context.Cars
                    .Select(x => new CarWithPartsOutputModel
                    {

                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                        Parts = x.PartCars.Select(pc => new MiniPartsOutputModel
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price,
                        })
                        .OrderByDescending(x => x.Price)
                        .ToArray(),
                    })
                    .OrderByDescending(x => x.TravelledDistance)
                    .ThenBy(x => x.Model)
                    .Take(5)
                .ToList();

            var result = XmlConverter.Serialize(cars, "cars");

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new LocalSuppliersOutputModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count(),
                })
                .ToList();

            var result = XmlConverter.Serialize(suppliers, "suppliers");

            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            InitializeMapper();
            var dbCars = context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance);

            var cars = mapper.Map<BMWCarOutputModel[]>(dbCars.ToList());

            var result = XmlConverter.Serialize(cars, "cars");


            return result;
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            InitializeMapper();
            var dbCars = context.Cars
                .Where(x => x.TravelledDistance > 2_000_000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10);

            var cars = mapper.Map<CarDistanceOutputModel[]>(dbCars.ToList());

            var result = XmlConverter.Serialize(cars, "cars");


            return result;
        }


        //Import methods
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            var carsIds = context.Cars.Select(x => x.Id);
            var customersIds = context.Customers.Select(x => x.Id);

            var salesDTOs = XmlConverter.Deserializer<SaleInputModel>(inputXml, "Sales")
                .Where(x => carsIds.Contains(x.CarId));

            var sales = mapper.Map<Sale[]>(salesDTOs);

            context.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();

            var customerDTOs = XmlConverter.Deserializer<CustomerInputModel>(inputXml, "Customers");

            var customers = mapper.Map<Customer[]>(customerDTOs);

            context.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            var partsIds = context.Parts.Select(x => x.Id);

            var carsDTOs = XmlConverter.Deserializer<CarInputModel>(inputXml, "Cars")
                .Select(x => new CarInputModel
                {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                    Parts = x.Parts.Select(x => x.PartId)
                .Distinct()
                .Intersect(partsIds)
                .Select(x => new MiniPartsInputModel { PartId = x })
                .ToArray(),
                });

            var cars = carsDTOs
                .Select(x => new Car
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TraveledDistance,
                    PartCars = x.Parts.Select(p => new PartCar { PartId = p.PartId }).ToList()
                });

            context.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            var xmlSerializer = new XmlSerializer(typeof(PartInputModel[]), new XmlRootAttribute("Parts"));
            var reader = new StringReader(inputXml);

            var partsDTOS = xmlSerializer.Deserialize(reader) as PartInputModel[];

            var supplierIds = context.Suppliers.Select(x => x.Id).ToList();
            var parts = mapper.Map<Part[]>(partsDTOS).Where(x => supplierIds.Contains(x.SupplierId));

            context.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count()}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            InitializeMapper();
            //Apparently bad to use this
            //var docRoot = XDocument.Parse(inputXml).Root;
            //mapper.Map<SupplierInputModel>(docRoot.Elements());
            var xmlSerializer = new XmlSerializer(typeof(SupplierInputModel[]), new XmlRootAttribute("Suppliers"));
            var reader = new StringReader(inputXml);
            var supplierDTOS = xmlSerializer.Deserialize(reader) as SupplierInputModel[];

            var suppliers = supplierDTOS.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter,
            });
            //var suppliers = mapper.Map<Supplier>(supplierDTOS);

            context.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}";
        }

        //Util Methods

        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}