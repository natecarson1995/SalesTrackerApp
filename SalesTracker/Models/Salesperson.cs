using System.Collections.Generic;

namespace SalesTracker
{
    /// <summary>
    /// Salesperson MVC Model class
    /// </summary>
    public class Salesperson
    {
        public enum Ranks
        {
            Beginner,
            Junior,
            Senior
        }
        #region FIELDS

        private Dictionary<Product.ProductType, Product> _currentStock;
        private int _age;
        private Ranks _rank;
        private string _firstName;
        private string _lastName;
        private string _accountID;
        private List<City> _citiesVisited;

        #endregion

        #region PROPERTIES

        public Dictionary<Product.ProductType, Product> CurrentStock
        {
            get { return _currentStock; }
            set { _currentStock = value; }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public Ranks Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
      
        public List<City> CitiesVisited
        {
            get { return _citiesVisited; }
            set { _citiesVisited = value; }
        }

        #endregion


        #region METHODS

        /// <summary>
        /// Initialize the properties of the three product types
        /// </summary>
        private void InitializeProducts()
        {
            //
            // Add a product to the list for each product type
            //
            foreach (var productType in Product.ProductType.GetValues(typeof(Product.ProductType)))
            {
                _currentStock.Add((Product.ProductType)productType, new Product((Product.ProductType)productType, 0));
            }

            //
            // Set product properties
            //
            _currentStock[Product.ProductType.Sedan].Color = "Red";
            _currentStock[Product.ProductType.Sedan].Price = 20000.00;
            _currentStock[Product.ProductType.Sedan].ModelName = "Accord";


            _currentStock[Product.ProductType.Truck].Color = "Black";
            _currentStock[Product.ProductType.Truck].Price = 40000.00;
            _currentStock[Product.ProductType.Truck].ModelName = "Ridgeline";


            _currentStock[Product.ProductType.Hatchback].Color = "Blue";
            _currentStock[Product.ProductType.Hatchback].Price = 30000.00;
            _currentStock[Product.ProductType.Hatchback].ModelName = "Civic";

            _currentStock[Product.ProductType.SUV].Color = "Green";
            _currentStock[Product.ProductType.SUV].Price = 35000.00;
            _currentStock[Product.ProductType.SUV].ModelName = "CR/V";
        }

        #endregion

        #region CONSTRUCTORS

        public Salesperson()
        {
            _citiesVisited = new List<City>();
            _currentStock = new Dictionary<Product.ProductType, Product>();
            InitializeProducts();
        }

        public Salesperson(string firstName, string lastName, string accountID)
        {
            _firstName = firstName;
            _lastName = lastName;
            _accountID = accountID;
            _citiesVisited = new List<City>();
            _currentStock = new Dictionary<Product.ProductType, Product>();
            InitializeProducts();
        }

        #endregion
        
    }
}
