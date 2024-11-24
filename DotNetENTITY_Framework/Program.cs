using DotNetENTITY_Framework.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace DotNetENTITY_Framework
{
    internal class Program
    {
        private static readonly object prod;

        static void Main()
        {
            Models.PMSDbEntities pMSDb = new Models.PMSDbEntities();
            //replaces all these lines

            //SqlConnection cn = new SqlConnection();

            //cn.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

            //SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            //csb.DataSource = "(localdb)\\MSSQLLocalDB";
            //csb.IntegratedSecurity = true;
            //csb.InitialCatalog = "LinQ_18Nov2024";
            //cn.ConnectionString = csb.ConnectionString;

            int choice = -1;
            do
            {
                Console.Clear();

                Console.WriteLine("1.List Products\n 2.Add Product\n 3.Update Product\n 4. Delete Product\n 5.Invoke stored Product\n 6.CRUD using dataset\n");

                Console.WriteLine("Enter Choice");
                choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                {
                    ListProductTable(pMSDb);

                }
                if (choice == 2)
                {
                    AddNewProduct(pMSDb);

                }

                if (choice == 3)
                {
                    UpdateProduct(pMSDb);

                }
                if (choice == 4)
                {
                    DeleteProduct(pMSDb);

                }
                if (choice == 5)
                {
                    StoredProcedureEF(pMSDb);

                }

            } while (choice != 1);
        }

        private static void StoredProcedureEF(PMSDbEntities pMSDb)
        {
            Console.Clear();
            Console.Write("enter product id: ");
            int prdId = int.Parse(Console.ReadLine());

            ObjectParameter objpara = new ObjectParameter("productName", "");

            pMSDb.GetPRoductNameByID(prdId, objpara);

            if (objpara.Value != null)
            {
                Console.WriteLine($" Product Name : {objpara.Value.ToString()}");

            }

            Console.WriteLine("Press Any Key to Continue");
            Console.ReadKey();

            Console.WriteLine("Product ID".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));

            Console.WriteLine("  ".PadLeft(10, ' ') + "  ".PadRight(30, ' ') + "  ".PadLeft(10, ' ') + "  ".PadLeft(10, ' '));

            foreach (var item in pMSDb.FetchProducts())
            {
                Console.WriteLine($"{item.ProductID.ToString().PadLeft(10, ' ')} {item.ProductName.PadLeft(20, ' ')}{item.quantity.ToString().PadLeft(10, ' ')}{item.Rate.ToString().PadLeft(10, ' ')}");
            }

            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }

        private static void DeleteProduct(PMSDbEntities pMSDb)
        {
            Models.Product product = new Models.Product();
            SeekProduct(pMSDb);

            if (product == null)
            {
                return;
            }
            Console.Write("are you sure? Y/N");
            string result = Console.ReadLine(); 

            if (result.ToUpper() == "N")
            {
                return;
            }
            pMSDb.Products.Remove(product);
            pMSDb.SaveChanges();
            Console.WriteLine("Deleted");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();


        }
        private static void UpdateProduct(PMSDbEntities pMSDb)
        {
            Models.Product product = new Models.Product();
            SeekProduct(pMSDb);

            if(product == null)
            {
                return;
            }

            Console.WriteLine("Edit Product Details");

            Console.WriteLine("Enter updated Product Name");
            product.ProductName = Console.ReadLine();

            Console.Write("Enter updated quality");
            product.quantity = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter updated rate");
            product.Rate = int.Parse(Console.ReadLine());

            pMSDb.SaveChanges();

            Console.WriteLine("update Successfuly");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();


        }

        private static void ListProductTable(PMSDbEntities pMSDb)
        {


            Console.WriteLine("Product ID".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));
            Console.WriteLine("_".PadRight(60, '_'));

            foreach (var item in pMSDb.Products)
            {
                Console.WriteLine(
                        $"{item.ProductID.ToString().PadLeft(10, ' ')}{item.ProductName.PadRight(30, ' ')}{item.quantity.ToString().PadLeft(10, ' ')}{item.Rate.ToString().PadLeft(10, ' ')}");
            }


            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();

        }

        private static void AddNewProduct(PMSDbEntities pMSDb)
        {
            Models.Product product = new Models.Product();
            Console.Clear();
            Console.WriteLine("Add New Product ");

            Console.Write("Enter Product Name:");
            product.ProductName = Console.ReadLine();

            Console.Write("Enter Quantity:");
            product.quantity = int.Parse(Console.ReadLine());

            Console.Write("Enter Rate:");
            product.Rate = int.Parse(Console.ReadLine());


            pMSDb.Products.Add(product);
            pMSDb.SaveChanges();




            Console.WriteLine("Inserted Successfully....");


            Console.WriteLine("Press Any key to continue...");
            Console.ReadKey();
        }

        private static void SeekProduct(PMSDbEntities pMSDb)
        {
            Models.Product product = new Models.Product();
            Console.Clear();
            Console.Write("Enter ProductName containing : ");
            string productname = Console.ReadLine();

            //   foreach (var item in pMSDb.Products.Where(p => p.ProductName.Contains(productname))) { }

            var qry = pMSDb.Products.Where(p => p.ProductName.Contains(productname));

            if (qry.Count() == 0)
            {
                //no record found
                { Console.WriteLine("No Records Found...\n Press Any Key to Continue"); Console.ReadKey(); product = null; return; }
            }
            //  to list products
            Console.WriteLine("Product ID".PadLeft(10, ' ') + "Product Name".PadRight(30, ' ') + "Quantity".PadLeft(10, ' ') + "Rate".PadLeft(10, ' '));
            Console.WriteLine("_".PadRight(60, '_'));

            qry.ToList().ForEach(p =>
            {
                Console.WriteLine($"{p.ProductID.ToString().PadLeft(10, ' ')} {p.ProductName.PadLeft(20, ' ')}{p.quantity.ToString().PadLeft(10, ' ')}{p.Rate.ToString().PadLeft(10, ' ')}");
            });


            if (qry.Count() > 1)
            {
                Console.WriteLine("Enter Product ID to edit : ");
                int prdId = int.Parse(Console.ReadLine());

                product = pMSDb.Products.SingleOrDefault(pd => pd.ProductID == prdId);


            }
            else { product = qry.FirstOrDefault(); }


        }
    }
}
