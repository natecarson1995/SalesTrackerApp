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
                sb.AppendLine(kv.Key + "," + kv.Value.Serialize());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string containing a list of serialized cities
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string Serialize(this List<City> cities)
        {
            //
            // initialize string builder
            //
            StringBuilder sb = new StringBuilder();

            //
            // build out string
            //
            foreach (City city in cities)
            {
                sb.AppendLine(city.Serialize());
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

        /// <summary>
        /// Deserializes a serialized salesperson, and adds the properties to it automatically
        /// </summary>
        /// <param name="salesperson"></param>
        /// <param name="serializedString"></param>
        /// <returns></returns>
        public static bool Deserialize(this Salesperson salesperson,string serializedString)
        {
            //
            // initialize variables
            //
            int age;
            Salesperson.Ranks rank;

            string[] parameters = serializedString.Split(',');

            if (parameters.Length < 4)
                throw new Exception("Invalid salesman parameters");

            //
            // validate the text file
            //
            if (int.TryParse(parameters[0], out age))
                salesperson.Age = age;
            else
                throw new Exception("Invalid or missing salesman age");

            salesperson.FirstName = parameters[1];
            salesperson.LastName = parameters[2];

            if (Enum.TryParse<Salesperson.Ranks>(parameters[3], out rank))
                salesperson.Rank = rank;
            else
                throw new Exception("Invalid or missing salesman rank");

            return true;
        }

        public static bool Deserialize(this Product product, string serializedString)
        {
            

            return true;
        }
        /// <summary>
        /// Deserializes a string containing a dictionary of products
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="serializedString"></param>
        /// <returns></returns>
        public static bool Deserialize(this Dictionary<Product.ProductType,Product> dict, string serializedString)
        {
            //
            // go through different products
            //
            string[] lines = serializedString.Split('\n');

            foreach (string line in lines)
            {
                //
                // split up line into product type (key) and actual product
                //
                int firstComma = line.IndexOf(',');
                Product.ProductType key;

                if (!Enum.TryParse<Product.ProductType>(line.Substring(0, firstComma), out key))
                    throw new Exception("Invalid product type");

                dict[key].Deserialize(line.Substring(firstComma + 1));
            }

            return true;
        }
    }
}
