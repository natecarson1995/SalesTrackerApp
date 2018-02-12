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

                    case MenuOption.SaveAccountInfo:
                        DisplaySaveAccountInfo();
                        break;

                    case MenuOption.LoadAccountInfo:
                        DisplayLoadAccountInfo();
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
            _consoleView.DisplaySaveAccountInfo(SaveAccountInfo());
        }

        /// <summary>
        /// load the salesman account info from a data file
        /// </summary>
        private void DisplayLoadAccountInfo()
        {
            _consoleView.DisplayLoadAccountInfo(LoadAccountInfo());
        }
        /// <summary>
        /// sets up the salesman account
        /// </summary>
        private void DisplaySetUpAccount()
        {
            _salesPerson = _consoleView.DisplaySetupAccount();
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
        /// Saves the account info to the data log, catching exceptions
        /// </summary>
        private string SaveAccountInfo()
        {
            //
            // make sure that file exists
            //
            if (!Directory.Exists("AccountInfo"))
            {
                try
                {
                    Directory.CreateDirectory("AccountInfo");

                    if (!File.Exists("AccountInfo/Data.csv"))
                        File.Create("AccountInfo/Data.csv");
                }
                catch (Exception)
                {
                    return "File/directory creation error";
                }
            }

            //
            // initialize variables
            //
            StringBuilder sb = new StringBuilder();
            StreamWriter sw;

            try
            {
                sw = new StreamWriter("AccountInfo/Data.csv");
            } catch (Exception)
            {
                return "File writing error";
            }

            //
            // build out the output string, catching exceptions
            //

            try
            {
                sb.AppendLine("Salesperson");
                sb.AppendLine(_salesPerson.Serialize());
                sb.AppendLine("Product");
                sb.Append(_salesPerson.CurrentStock.Serialize());
                sb.AppendLine("Cities");
                sb.AppendLine(_salesPerson.CitiesVisited.Serialize());
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


            //
            // save the output string to the text file
            //
            try
            {
                using (sw)
                {
                    sw.Write(sb);
                }
            } catch (Exception)
            {
                return "File Write Error";
            }
            return "File saved succesfully";
        }

        /// <summary>
        /// Loads the account info from the data log, validating and catching exceptions
        /// </summary>
        private string LoadAccountInfo()
        {
            //
            // make sure file exists
            //
            if (Directory.Exists("AccountInfo"))
            {
                if (!File.Exists("AccountInfo/Data.csv"))
                    return "File not found error";
            }
            else
                return "Directory not found error";

            //
            // initialize variables
            //
            int cityLine=0;
            StreamReader sr = new StreamReader("AccountInfo/Data.csv");
            StringBuilder sb = new StringBuilder();
            string[] dataInLines;

            //
            // split up textfile by lines
            //
            try
            {
                using (sr)
                {
                    dataInLines = sr.ReadToEnd().TrimEnd().Split('\n');
                }
            } catch (Exception)
            {
                return "File access error";
            }

            //
            // Because cities and products take up multiple lines, we split them up with a line containing the word "cities"
            //
            for (int i = 0; i < dataInLines.Length; i++)
            {
                dataInLines[i] = dataInLines[i].Trim();
                if (dataInLines[i] == "Cities")
                {
                    cityLine = i;
                }
            }
            //
            // deserialize salesperson
            //
            try
            {
                _salesPerson.Deserialize(dataInLines[1]);
            } catch (Exception)
            {
                return "Salesperson parsing error";
            }

            //
            // put all the lines for products into one big string and deserialize
            //
            sb.Clear();

            try
            {
                for (int i=3;i<cityLine;i++)
                {
                    sb.AppendLine(dataInLines[i]);
                }
                _salesPerson.CurrentStock.Deserialize(sb.ToString());
            }
            catch (Exception)
            {
                return "Product parsing error";
            }
            //
            // put all the lines for cities into one big string and deserialize
            //
            sb.Clear();

            try
            {
                for (int i=cityLine+1;i<dataInLines.Length;i++)
                {
                    sb.AppendLine(dataInLines[i]);
                }
                _salesPerson.CitiesVisited.Deserialize(sb.ToString());
            }
            catch (Exception)
            {
                return "City parsing error";
            }

            return "File loaded succesfully";
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
