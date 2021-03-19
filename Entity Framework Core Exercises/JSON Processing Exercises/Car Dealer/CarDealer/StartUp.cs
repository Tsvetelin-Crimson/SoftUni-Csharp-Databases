using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;
        static JsonSerializerSettings jsonSerializerSettings;

        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");

            //ImportSuppliers(db, suppliersJson);
            //ImportParts(db, partsJson);
            //ImportCars(db, carsJson);
            //ImportCustomers(db, customersJson);
            //ImportSales(db, salesJson);
            Console.WriteLine(GetTotalSalesByCustomer(db));
        }
        //Export Methods

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance,
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("F2"),
                    price = x.Car.PartCars.Sum(pc => pc.Part.Price).ToString("F2"),
                    priceWithDiscount = (x.Car.PartCars
                    .Sum(pc => pc.Part.Price) * (x.Discount == 0 ? 1 : (100 - x.Discount) / 100)).ToString("F2"),

                })
                .Take(10)
                .ToList();

            var resultJson = JsonSerialize(sales);
            return resultJson;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Count() > 0)
                .Select(x => new
                {
                    fullName = x.Name,
                    boughtCars = x.Sales.Count(),
                    spentMoney = x.Sales
                                .Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                                .ToString("F2") 
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToList();

            var resultJson = JsonSerialize(customers);
            return resultJson;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(x => new
                {
                    car = new {
                        x.Make,
                        x.Model,
                        x.TravelledDistance,
                    },
                    parts = x.PartCars.Select(pc => new
                    {
                        pc.Part.Name,
                        Price = pc.Part.Price.ToString("F2"),
                    }),
                })
                .ToList();

            var resultJson = JsonSerialize(cars);
            return resultJson;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count(),
                })
                .ToList();

            var resultJson = JsonSerialize(suppliers);
            return resultJson;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars.Where(x => x.Make == "Toyota")
               .OrderBy(x => x.Model)
               .ThenByDescending(x => x.TravelledDistance)
               .Select(x => new
               {
                   x.Id,
                   x.Make,
                   x.Model,
                   x.TravelledDistance,
               })
               .ToList();

            var resultJson = JsonSerialize(cars);
            return resultJson;
        }


        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    x.IsYoungDriver,
                })
                .ToList();

            var resultJson = JsonSerialize(customers);
            return resultJson;
        }


        //Import Methods
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var carsIds = context.Cars.Select(x => x.Id);
            var customersIds = context.Customers.Select(x => x.Id);

            var allSaleModels = JsonConvert.DeserializeObject<IEnumerable<Sale>>(inputJson);

            //Completely unneeded????????
            //var salesModels = new List<Sale>();

            //foreach (var sale in allSaleModels)
            //{
            //    bool carIdIsValid = false;
            //    bool customerIdIsValid = false;
            //    var currSale = new Sale()
            //    {
            //        Discount = sale.Discount
            //    };

            //    if (carsIds.Contains(sale.CarId))
            //    {
            //        carIdIsValid = true;
            //        currSale.CarId = sale.CarId;
            //    }

            //    if (customersIds.Contains(sale.CustomerId))
            //    {
            //        customerIdIsValid = true;
            //        currSale.CustomerId = sale.CustomerId;
            //    }

            //    if (carIdIsValid && customerIdIsValid)
            //    {
            //        salesModels.Add(currSale);
            //    }

            //}

            var sales = mapper.Map<IEnumerable<Sale>>(allSaleModels);

            context.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }
        
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var allCustomerModels = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(inputJson);

            var customers = mapper.Map<IEnumerable<Customer>>(allCustomerModels);

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }
        
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var partsIds = context.Parts.Select(x => x.Id);
            var allCarModels = JsonConvert.DeserializeObject<IEnumerable<CarsModel>>(inputJson);

            var cars = new List<Car>();
            foreach (var car in allCarModels)
            {
                var currCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                    
                };
                foreach (var part in car.PartsId.Distinct())
                {
                    if (partsIds.Contains(part))
                    {
                        currCar.PartCars.Add(new PartCar
                        {
                            PartId = part
                        });
                    }
                    
                }

                cars.Add(currCar);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {allCarModels.Count()}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliersIds = context.Suppliers.Select(x => x.Id);


            var allPartsModels = JsonConvert.DeserializeObject<IEnumerable<PartModel>>(inputJson)
                .Where(x => suppliersIds.Contains(x.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(allPartsModels);

            context.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var supplierModels = JsonConvert.DeserializeObject<IEnumerable<SupplierModel>>(inputJson);
            var suppliers = mapper.Map<IEnumerable<Supplier>>(supplierModels);

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }



        // Util Methods
        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }

        private static string JsonSerialize(object value, bool shouldIgnoreNullValues = false)
        {
            InitializeJsonSerializerSettings();
            if (shouldIgnoreNullValues)
            {
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }
        private static void InitializeJsonSerializerSettings()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new DefaultNamingStrategy()
            };
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
        }
    }
}