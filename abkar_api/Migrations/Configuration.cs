using System;
using System.Data.Entity.Migrations;
using abkar_api.Models;
using Faker;
using FizzWare.NBuilder;
using System.Linq;
using System.Collections.Generic;

namespace abkar_api.Migrations
{
    
    internal sealed class Configuration : DbMigrationsConfiguration<abkar_api.Models.MigrationContexts>
    {
        bool EnableDummyData = true;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(abkar_api.Models.MigrationContexts context)
        {
            if(EnableDummyData)
            {

                var customer = Builder<Customers>.CreateListOfSize(40)
                    .All()
                    .With(c => c.name = Faker.Name.First())
                    .With(c => c.lastname = Faker.Name.Last())
                    .With(c => c.phone = Faker.Phone.Number())
                    .With(c => c.email = Faker.Internet.Email())
                    .With(c => c.company = Faker.Company.Name())
                    .With(c => c.adress = Faker.Address.StreetAddress() + " " + Faker.Address.StreetName() + " " + Faker.Address.ZipCode())
                    .With(c => c.city = "Tekirdað")
                    .With(c => c.town = "Çorlu")
                    .With(c => c.password = Faker.RandomNumber.Next(9999,999999).ToString())
                    .With(c => c.state = true)
                    .Build();

                var personel = Builder<Personnel>.CreateListOfSize(20)
                    .All()
                    .With(p => p.name = Faker.Name.First())
                    .With(p => p.lastname = Faker.Name.Last())
                    .With(p => p.password = Faker.RandomNumber.Next(100, 999).ToString())
                    .With(p => p.username = Faker.Internet.UserName())
                    .With(p => p.state = true)
                    .With(p => p.department_id = Faker.RandomNumber.Next(1, 4))
                    .Build();

               List<String> types = new List<string>();
                types.Add("Döküm");
                types.Add("Hammadde");
                types.Add("Mamul");
                types.Add("Yarý Mamul");

          

                var stockcard = Builder<StockCards>.CreateListOfSize(30)
                     .All()
                     .With(sc => sc.code = "Kapak " + Faker.RandomNumber.Next(999, 9999).ToString())
                     .With(sc => sc.stock_type =  types[Faker.RandomNumber.Next(0,4)])
                     .With(sc => sc.unit = Faker.RandomNumber.Next(50, 200))
                     .With(sc => sc.name = Faker.Lorem.Sentence(2))
                     .Build();



               var supplier = Builder<Suppliers>.CreateListOfSize(40)
               .All()
               .With(c => c.name = Faker.Name.First())
               .With(c => c.lastname = Faker.Name.Last())
               .With(c => c.phone = Faker.Phone.Number())
               .With(c => c.email = Faker.Internet.Email())
               .With(c => c.company = Faker.Company.Name())
               .With(c => c.adress = Faker.Address.StreetAddress() + " " + Faker.Address.StreetName() + " " + Faker.Address.ZipCode())
               .With(c => c.city = "Tekirdað")
               .With(c => c.town = "Çorlu")
               .Build();


                context.Customer.AddOrUpdate(c => c.id, customer.ToArray());
                context.Personnel.AddOrUpdate(p => p.id, personel.ToArray());
                context.StockCards.AddOrUpdate(sc => sc.id, stockcard.ToArray());
                context.Suppliers.AddOrUpdate(s => s.id, supplier.ToArray());


            }

            context.StockTypes.AddOrUpdate(s => s.name,
               new StockTypes { name = "Döküm" },
               new StockTypes { name = "Hammadde" },
               new StockTypes { name = "Yarý Mamul" },
               new StockTypes { name = "Mamul" }
           );

            
            context.Machines.AddOrUpdate(s => s.name,
                 new Machines { name = "Tekowa T20" },
                 new Machines { name = "Hynadia A40" },
                 new Machines { name = "Honda DD125" },
                 new Machines { name = "Arion 120" }
            );

            context.Operations.AddOrUpdate(s => s.name,
              new Operations { name = "Kalite Kontrol", operation_time = 3},
              new Operations { name = "Delme", operation_time = 2 },
              new Operations { name = "Temizleme", operation_time = 4 },
              new Operations { name = "Delik Açma", operation_time = 7 }
            );


            context.Deparments.AddOrUpdate(d => d.name,
                new Departments { name = "Planlama", created_date = DateTime.Now, role = "planning" },
                new Departments { name = "Operasyon", created_date = DateTime.Now, role = "operation" },
                new Departments { name = "Kalite Kontrol", created_date = DateTime.Now, role = "quality" },
                new Departments { name = "Yönetim", created_date = DateTime.Now, role = "admin" }
            );
        }
    }
}
