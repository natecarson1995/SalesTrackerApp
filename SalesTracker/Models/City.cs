using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker
{
    public class City
    {
        #region Fields

        private DateTime _dateArrive;
        private DateTime _dateDepart;
        private int _productsSold;
        private int _productsBought;
        private string _name;
        private List<string> _companiesVisited;

        #endregion

        #region Properties

        public DateTime DateArrive
        {
            get { return _dateArrive; }
            set { _dateArrive = value; }
        }
        public DateTime DateDepart
        {
            get { return _dateDepart; }
            set { _dateDepart = value; }
        }
        public int ProductsBought
        {
            get { return _productsBought; }
        }
        public int ProductsSold
        {
            get { return _productsSold; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<string> CompaniesVisited
        {
            get { return _companiesVisited; }
            set { _companiesVisited = value; }
        }

        #endregion

        #region Methods
        
        //
        // Gets the total days spent in the city, returning -1 if not yet departed
        //
        public int DaysSpent()
        {
            if (Departed())
            {
                return (int)(DateDepart - DateArrive).TotalDays;
            }
            else return -1;
        }

        //
        // Returns whether or not the salesman has departed the city yet
        //
        public bool Departed()
        {
            return DateDepart != default(DateTime);
        }

        //
        // Add products bought to the record for the city
        //
        public void BuyProducts(int productNumber)
        {
            _productsBought += productNumber;
        }

        //
        // Add products sold to the record for the city
        //
        public void SellProducts(int productNumber)
        {
            _productsSold += productNumber;
        }
        
        //
        // Set the departure date in the record
        //
        public void Depart()
        {
            _dateDepart = DateTime.Now;
        }

        #endregion

        #region Constructors

        public City(string name)
        {
            _name = name;
            _companiesVisited = new List<string>();
            _dateArrive = DateTime.Now;
        }

        #endregion
    }
}
