using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker
{
    public static class SerializationUtil
    {
        /// <summary>
        /// Returns a string containing the serialized product list
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string Serialize(this Dictionary<Product.ProductType, Product> dict)
        {
            //
            // initialize string builder
            //
            StringBuilder sb = new StringBuilder();

            //
            // build out string
            //
            foreach (var kv in dict)
            {
                sb.Append(kv.Value.Serialize());
            }

            return sb.ToString();
        }


        /// <summary>
        /// Returns a string containing a single serialized city
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string Serialize(this City city)
        {
            //
            // initialize string builder
            //
            StringBuilder sb = new StringBuilder();

            //
            // build out string
            //
            sb.Append(city.DateArrive + ","
                + city.DateDepart+ ","
                + city.Name + ","
                + city.ProductsBought + ","
                + city.ProductsSold + ","
                );

            foreach (string company in city.CompaniesVisited)
            {
                sb.Append(company + ",");
            }
            sb.Remove(sb.Length - 1,1);

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string containing a single serialized product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string Serialize(this Product product)
        {
            //
            // initialize string builder
            //
            StringBuilder sb = new StringBuilder();

            //
            // build out string
            //
            sb.Append(product.Color + ","
                + product.ModelName + ","
                + product.NumberOfUnits + ","
                + product.Price + ","
                + product.Type);

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string containing a single serialized salesperson
        /// </summary>
        /// <param name="salesperson"></param>
        /// <returns></returns>
        public static string Serialize(this Salesperson salesperson)
        {
            //
            // initialize string builder
            //
            StringBuilder sb = new StringBuilder();

            //
            // build out string
            //
            sb.Append(salesperson.AccountID + ","
                + salesperson.Age + ","
                + salesperson.FirstName + ","
                + salesperson.LastName + ","
                + salesperson.Rank);

            return sb.ToString();
        }
    }
}
