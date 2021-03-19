using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;
        static JsonSerializerSettings jsonSerializerSettings;

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //string productJson = File.ReadAllText("../../../Datasets/products.json");
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //string categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //ImportUsers(db, usersJson);
            //ImportProducts(db, productJson);
            //ImportCategories(db, categoriesJson);
            //ImportCategoryProducts(db, categoriesProductsJson);
            Console.WriteLine(GetUsersWithProducts(db));
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ThenInclude(x => x.Buyer)
                .ToList()
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Where(ps => ps.BuyerId != null).Count(),
                        Products = x.ProductsSold.Where(sp => sp.BuyerId != null).Select(sp => new
                        {
                            sp.Name,
                            sp.Price,
                        }),
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Products.Count())
                .ToList();

            var endObj = new
            {
                UsersCount = users.Count(),
                Users = users,
            };


            string returnJson = JsonSerialize(endObj, true);
            return returnJson;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count())
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(cp => cp.Product.Price).ToString("F2"),
                    TotalRevenue = x.CategoryProducts.Sum(cp => cp.Product.Price).ToString("F2"),
                })
                .ToList();


            string returnJson = JsonSerialize(categories);
            return returnJson;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    x.Name,
                    x.Price,
                    Seller = x.Seller.FirstName + " " + x.Seller.LastName,
                })
                .ToList();

            string returnJson = JsonSerialize(products);


            //var returnJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            return returnJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    SoldProducts = x.ProductsSold.Where(sp => sp.BuyerId != null).Select(sp => new
                    {
                        sp.Name,
                        sp.Price,
                        BuyerFirstName = sp.Buyer.FirstName,
                        BuyerLastName = sp.Buyer.LastName,
                    })
                })
                .ToList();

            string returnJson = JsonSerialize(users);
            return returnJson;

        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var categoriesProducts = JsonConvert.DeserializeObject<IEnumerable<CategoryProductInputModel>>(inputJson);
            var mainCategoriesProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoriesProducts);


            context.CategoryProducts.AddRange(mainCategoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {mainCategoriesProducts.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var categories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson)
                .Where(x => x.Name != null);

            var mainCategories = mapper.Map<IEnumerable<Category>>(categories);


            context.Categories.AddRange(mainCategories);
            context.SaveChanges();
            return $"Successfully imported {mainCategories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var products = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            var mainProducts = mapper.Map<IEnumerable<Product>>(products);


            context.Products.AddRange(mainProducts);
            context.SaveChanges();
            return $"Successfully imported {mainProducts.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            InitializeMapper();

            var users = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            var mainUsers = mapper.Map<IEnumerable<User>>(users);
            

            context.Users.AddRange(mainUsers);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }


        //Util methods
        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = config.CreateMapper();
        }

        private static string JsonSerialize(object value, bool shouldIgnoreNullValues = false) // will need the same naming strategy here (2)
        {
            InitializeJsonSerializerSettings();
            if (shouldIgnoreNullValues)
            {
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }
        private static void InitializeJsonSerializerSettings() //could put the naming strategy class here as a paramater (1)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
        }
    }
}