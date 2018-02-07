using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker
{
    public class Product
    {
        public enum ProductType
        {
            Sedan,
            Hatchback,
            Truck,
            SUV
        };

        #region Fields

        private double _price;
        private int _numberOfUnits;
        private ProductType _type;
        private string _color;
        private string _modelName;

        #endregion

        #region Properties

        public bool OnBackOrder
        {
            get { return _numberOfUnits<0; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public int NumberOfUnits
        {
            get { return _numberOfUnits; }
            set { _numberOfUnits = value; }
        }
        public ProductType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add units to the stock
        /// </summary>
        /// <param name="unitsToAdd"></param>
        public void AddProducts(int unitsToAdd)
        {
            _numberOfUnits += unitsToAdd;
        }
        /// <summary>
        /// Subtract units from the stock
        /// </summary>
        /// <param name="unitsToSubtract"></param>
        public void SubtractProducts(int unitsToSubtract)
        {
            _numberOfUnits -= unitsToSubtract;
        }

        #endregion

        #region Constructors

        public Product() { }
        public Product(ProductType type, int numberOfUnits)
        {
            _type = type;
            _numberOfUnits = numberOfUnits;
        }

        #endregion
    }
}
