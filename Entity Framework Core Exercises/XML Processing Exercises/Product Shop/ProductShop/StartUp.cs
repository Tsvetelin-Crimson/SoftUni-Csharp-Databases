using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DataTransferObjects.Input;
using ProductShop.DataTransferObjects.Output;
using ProductShop.Models;
using ProductShop.XmlHelper;
using System;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var usersXML = File.ReadAllText("Datasets/users.xml");
            //var productsXML = File.ReadAllText("Datasets/products.xml");
            //var categoriesXML = File.ReadAllText("Datasets/categories.xml");
            //var categoriesProductsXML = File.ReadAllText("Datasets/categories-products.xml");


            //ImportUsers(context, usersXML);
            //ImportProducts(context, productsXML);
            //ImportCategories(context, categoriesXML);
            //ImportCategoryProducts(context, categoriesProductsXML);
            Console.WriteLine(GetUsersWithProducts(context));
        }
        //Export Methods

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersAndProducts = context.Users
                .Include(x => x.ProductsSold)
                .ThenInclude(x => x.Buyer)
                .ToList()
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new MiniUserOutputModel
                {
                    
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                     Age = x.Age,
                    SoldProducts = new MiniSoldProductOutputModel
                    {
                        Count = x.ProductsSold.Where(ps => ps.BuyerId != null).Count(),
                        Products = x.ProductsSold.Where(sp => sp.BuyerId != null)
                        .Select(sp => new MiniProductNPOutputModel
                        {
                            Name = sp.Name,
                            Price = sp.Price,
                        })
                        .OrderByDescending(sp => sp.Price)
                        .ToArray(),
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Products.Count());

            var UsersProductsAndCount = new UserAndProductOutputModel
            {
                Count = usersAndProducts.Count(),
                Users = usersAndProducts.Take(10).ToArray(),
            };

            var result = XmlConverter.Serialize(UsersProductsAndCount, "Users");

            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var sales = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count())
                .Select(x => new CategoriesByProductCountOutputModel
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(cp => cp.Product.Price),
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();


            var result = XmlConverter.Serialize(sales, "Categories");

            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var sales = context.Users
               .Where(x => x.ProductsSold.Count() > 0)
               .Select(x => new SoldProductsOutputModel
               {
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   SoldProducts = x.ProductsSold.Select(ps => new MiniProductOutputModel
                   {
                       Name = ps.Name,
                       Price = ps.Price,
                   })
                   .ToArray(),
               })
               .OrderBy(x => x.LastName)
               .ThenBy(x => x.FirstName)
               .Take(5)
               .ToList();


            var result = XmlConverter.Serialize(sales, "Users");

            return result;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var sales = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ProductsInRangeOutputModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToList();


            var result = XmlConverter.Serialize(sales, "Products");

            return result;
        }

        //Import methods

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            var productsIds = context.Products.Select(x => x.Id);
            var categoriesIds = context.Categories.Select(x => x.Id);

            var productDTOs = XmlConverter.Deserializer<CategoryProductInputModel>(inputXml, "CategoryProducts")
                .Where(x => productsIds.Contains(x.ProductId) && categoriesIds.Contains(x.CategoryId));

            var product = mapper.Map<CategoryProduct[]>(productDTOs);

            context.AddRange(product);
            //context.SaveChanges();
            return $"Successfully imported {product.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            var categoriesDTOs = XmlConverter.Deserializer<CategoryInputModel>(inputXml, "Categories")
                .Where(x => x.Name != null);

            var categories = mapper.Map<Category[]>(categoriesDTOs);

            context.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            var userIds = context.Users.Select(x => x.Id);
            var productDTOs = XmlConverter.Deserializer<ProductInputModel>(inputXml, "Products")
                .Where(x => userIds.Contains(x.BuyerId));

            var product = mapper.Map<Product[]>(productDTOs);

            context.AddRange(product);
            context.SaveChanges();
            return $"Successfully imported {product.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            InitializeMapper();
            var usersDTOs = XmlConverter.Deserializer<UserInputModel>(inputXml, "Users");

            var users = mapper.Map<User[]>(usersDTOs);

            context.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }


        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}