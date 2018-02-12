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
            foreach (var key in dict.Keys)
            {
                sb.AppendLine(key + "," + dict[key].Serialize());
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

            sb.Append(city.DateArrive.Ticks + ",");
            sb.Append(city.DateDepart.Ticks + ",");
            sb.Append(city.Name + ",");
            sb.Append(city.ProductsBought + ",");
            sb.Append(city.ProductsSold+",");

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

            sb.Append(product.Color+ ",");
            sb.Append(product.ModelName + ",");
            sb.Append(product.NumberOfUnits+ ",");
            sb.Append(product.Price + ",");
            sb.Append(product.Type);

            return sb.ToString().TrimEnd();
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
            sb.Append(salesperson.AccountID + ",");
            sb.Append(salesperson.Age + ",");
            sb.Append(salesperson.FirstName + ",");
            sb.Append(salesperson.LastName + ",");
            sb.Append(salesperson.Rank);

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

            if (!serializedString.Contains(','))
                throw new Exception("Invalid salesman format");

            string[] parameters = serializedString.Split(',');

            if (parameters.Length < 4)
                throw new Exception("Invalid salesman parameters");

            //
            // validate the text file
            //

            salesperson.AccountID = parameters[0];

            if (int.TryParse(parameters[1], out age))
                salesperson.Age = age;
            else
                throw new Exception("Invalid or missing salesman age");

            salesperson.FirstName = parameters[2];
            salesperson.LastName = parameters[3];

            if (Enum.TryParse<Salesperson.Ranks>(parameters[4], out rank))
                salesperson.Rank = rank;
            else
                throw new Exception("Invalid or missing salesman rank");

            return true;
        }

        public static bool Deserialize(this Product product, string serializedString)
        {
            int quantity;
            double price;
            Product.ProductType type;

            if (!serializedString.Contains(','))
                throw new Exception("Invalid product format");
            //
            // split up string into parameters
            //
            string[] parameters = serializedString.Split(',');
            if (parameters.Length<5)
                throw new Exception("Invalid product parameters");

            //
            // validate and set parameters
            //
            product.Color = parameters[0];
            product.ModelName = parameters[1];

            if (int.TryParse(parameters[2], out quantity))
                product.NumberOfUnits = quantity;
            else
                throw new Exception("Invalid product quantity");
            
            if (double.TryParse(parameters[3], out price))
                product.Price = price;
            else
                throw new Exception("Invalid product price");

            if (Enum.TryParse<Product.ProductType>(parameters[4], out type))
                product.Type = type;
            else
                throw new Exception("Invalid product type");

            return true;
        }

        public static bool Deserialize(this City city, string serializedString)
        {
            //
            // initialize variables
            //
            long arrival;
            long departure;
            int productsBought;
            int productsSold;

            if (!serializedString.Contains(','))
                throw new Exception("Invalid city format");

            string[] parameters = serializedString.Split(',');
            if (parameters.Length < 5)
                throw new Exception("Invalid city parameters");

            //
            // parse out and validate parameters
            //
            if (long.TryParse(parameters[0], out arrival))
                city.DateArrive = new DateTime(arrival);
            else
                throw new Exception("Invalid city arrival date");

            if (long.TryParse(parameters[1], out departure))
            {
                //
                // If they haven't departed yet, dont put a departure date in
                //
                if (departure == 0) city.DateDepart = default(DateTime);
                else city.DateDepart = new DateTime(departure);
            }
            else
                throw new Exception("Invalid city departure date");

            city.Name = parameters[2];

            if (int.TryParse(parameters[3], out productsBought))
                city.BuyProducts(productsBought);

            if (int.TryParse(parameters[4], out productsSold))
                city.SellProducts(productsSold);

            for (int i=5;i<parameters.Length;i++)
            {
                city.CompaniesVisited.Add(parameters[i]);
            }

            return true;
        }

        /// <summary>
        /// Deserializes a string containing a list of cities visited
        /// </summary>
        /// <param name="cities"></param>
        /// <param name="serializedString"></param>
        /// <returns></returns>
        public static bool Deserialize(this List<City> cities, string serializedString)
        {
            //
            // dont try to iterate an empty string
            //
            if (serializedString == "" || !serializedString.Contains('\n'))
                return true;

            //
            // go through different cities
            //
            string[] lines = serializedString.TrimEnd().Split('\n');

            foreach (string line in lines)
            {
                //
                // set up new city object for each line, and add to the cities visited
                //
                City city = new City("");
                city.Deserialize(line);
                cities.Add(city);
            }

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
            // dont try to iterate an empty string
            //
            if (serializedString == "" || !serializedString.Contains('\n'))
                return true;

            //
            // go through different products
            //
            string[] lines = serializedString.TrimEnd().Split('\n');

            foreach (string line in lines)
            {
                //
                // split up line into product type (key) and actual product
                //
                int firstComma = line.IndexOf(',');
                Product.ProductType key;

                if (!Enum.TryParse<Product.ProductType>(line.Substring(0, firstComma), out key))
                    throw new Exception("Invalid product type");
                else
                {
                    //
                    // deserialize each product
                    //
                    Product p = new Product();
                    p.Deserialize(line.Substring(firstComma + 1).Trim());

                    //
                    // either add or set the value in the dictionary
                    //
                    if (dict.ContainsKey(key))
                        dict[key] = p;
                    else
                        dict.Add(key, p);

                }
            }

            return true;
        }
    }
}
