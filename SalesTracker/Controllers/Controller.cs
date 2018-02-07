using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker
{
    /// <summary>
    /// MVC Controller class
    /// </summary>
    public class Controller
    {
        #region FIELDS
        private bool _usingApplication;
        private ConsoleView _consoleView;
        private Salesperson _salesPerson;
        
        #endregion

        #region PROPERTIES


        #endregion

        #region CONSTRUCTORS

        public Controller()
        {
            InitializeController();

            //
            // instantiate a Salesperson object
            //
            _salesPerson = new Salesperson();

            //
            // instantiate a ConsoleView object
            //
            _consoleView = new ConsoleView();

            //
            // begins running the application UI
            //
            ManageApplicationLoop();
        }

        #endregion
        
        #region METHODS

        /// <summary>
        /// initialize the controller 
        /// </summary>
        private void InitializeController()
        {
            _usingApplication = true;
        }

        /// <summary>
        /// method to manage the application setup and control loop
        /// </summary>
        private void ManageApplicationLoop()
        {
            MenuOption userMenuChoice;
            
            _consoleView.DisplayWelcomeScreen();

            //
            // application loop
            //
            while (_usingApplication)
            {
                //
                // get a menu choice from the user
                //
                userMenuChoice = _consoleView.DisplayGetUserMenuChoice();

                //
                // choose an action based on the user menu choice
                //
                switch (userMenuChoice)
                {
                    case MenuOption.None:
                        break;

                    case MenuOption.SetupAccount:
                        DisplaySetUpAccount();
                        break;

                    case MenuOption.Travel:
                        Travel();
                        break;

                    case MenuOption.DisplayCities:
                        DisplayCities();
                        break;

                    case MenuOption.DisplayAccountInfo:
                        DisplayAccountInfo();
                        break;

                    case MenuOption.EditAccountInfo:
                        DisplayEditAccountInfo();
                        break;

                    case MenuOption.DisplayInventory:
                        DisplayInventory();
                        break;

                    case MenuOption.Buy:
                        Buy();
                        break;

                    case MenuOption.Sell:
                        Sell();
                        break;

                    case MenuOption.VisitCompany:
                        VisitCompany();
                        break;

                    case MenuOption.Exit:
                        _usingApplication = false;
                        break;

                    default:

                        break;
                }

            }

            _consoleView.DisplayClosingScreen();

            //
            // close the application
            //
            Environment.Exit(1);
        }

        /// <summary>
        /// saves the salesman account info to a data file
        /// </summary>
        private void DisplaySaveAccountInfo()
        {
            string status = "Sucessfully Saved";
            SaveAccountInfo();
            _consoleView.DisplaySaveAccountInfo(status);
        }

        /// <summary>
        /// load the salesman account info from a data file
        /// </summary>
        private void DisplayLoadAccountInfo()
        {
            string status = "Sucessfully Loaded";
            LoadAccountInfo();
            _consoleView.DisplayLoadAccountInfo(status);
        }
        /// <summary>
        /// sets up the salesman account
        /// </summary>
        private void DisplaySetUpAccount()
        {
            _consoleView.DisplaySetupAccount();
        }
        /// <summary>
        /// add the next city location to the list of cities
        /// </summary>
        private void Travel()
        {
            if (_salesPerson.CitiesVisited.Count>0) _salesPerson.CitiesVisited.Last().Depart();
            City city = new City(_consoleView.DisplayGetNextCity());
            _salesPerson.CitiesVisited.Add(city);
        }

        /// <summary>
        /// display all cities traveled to
        /// </summary>
        private void DisplayCities()
        {
            _consoleView.DisplayCitiesTraveled(_salesPerson);
        }

        /// <summary>
        /// display account information
        /// </summary>
        private void DisplayAccountInfo()
        {
            _consoleView.DisplayAccountInfo(_salesPerson);
        }


        /// <summary>
        /// edit account information
        /// </summary>
        private void DisplayEditAccountInfo()
        {
            _consoleView.DisplayEditAccountInfo(_salesPerson);
        }

        /// <summary>
        /// Gets the number of units to buy from the user, then applies it to the inventory
        /// </summary>
        private void Buy()
        {
            Product.ProductType productType;
            //
            // Get the number of units to buy from the view
            //
            int numToBuy = _consoleView.DisplayGetNumberOfUnitsToBuy(out productType);

            //
            // Add that number to the product stock
            //
            if (numToBuy > 0)
            {
                _salesPerson.CurrentStock[productType].AddProducts(numToBuy);
                if (_salesPerson.CitiesVisited.Count > 0)
                    _salesPerson.CitiesVisited.Last().BuyProducts(numToBuy);
            }
        }

        /// <summary>
        /// Gets the number of units to sell from the user, then applies it to the inventory
        /// </summary>
        private void Sell()
        {
            Product.ProductType productType;
            //
            // Get the number of units to sell from the view
            //
            int numToSell = _consoleView.DisplayGetNumberOfUnitsToSell(out productType);

            //
            // Subtract that number from the product stock
            //
            if (numToSell > 0)
            {
                _salesPerson.CurrentStock[productType].SubtractProducts(numToSell);
                if (_salesPerson.CitiesVisited.Count > 0)
                    _salesPerson.CitiesVisited.Last().SellProducts(numToSell);
            }

            //
            // If there is a back order, warn the user
            //
            if (_salesPerson.CurrentStock[productType].OnBackOrder) _consoleView.DisplayBackorderNotification(_salesPerson.CurrentStock[productType]);
        }

        /// <summary>
        /// Saves the account info to the data log
        /// </summary>
        private void SaveAccountInfo()
        {
            //
            // initialize variables
            //
            StringBuilder sb = new StringBuilder();
            StreamWriter sw = new StreamWriter("AccountInfo/Data.csv");

            //
            // build out the output string
            //
            sb.AppendLine(_salesPerson.Serialize());
            sb.AppendLine(_salesPerson.CurrentStock.Serialize());
            

            //
            // save the output string to the text file
            //
            using (sw)
            {
                sw.Write(sb);
            }
        }
        /// <summary>
        /// Loads the account info from the data log
        /// </summary>
        private void LoadAccountInfo()
        {
            //TODO
        }
        /// <summary>
        /// Calls the view inventory display
        /// </summary>
        private void DisplayInventory()
        {
            _consoleView.DisplayInventory(_salesPerson.CurrentStock);
        }



        /// <summary>
        /// Gets company visited from view, and adds it to the list
        /// </summary>
        private void VisitCompany()
        {
            string company = _consoleView.DisplayGetCompanyVisited();
            if (_salesPerson.CitiesVisited.Count>0)
                _salesPerson.CitiesVisited.Last().CompaniesVisited.Add(company);
        }

        #endregion
    }
}
