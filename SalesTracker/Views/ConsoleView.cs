using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker
{
    /// <summary>
    /// MVC View class
    /// </summary>
    public class ConsoleView
    {
        #region FIELDS

        private const int MAXIMUM_ATTEMPTS=5;
        private const int MAXIMUM_BUYSELL_AMOUNT=50;
        private const int MINIMUM_BUYSELL_AMOUNT=1;

        #endregion

        #region PROPERTIES

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// default constructor to create the console view objects
        /// </summary>
        public ConsoleView()
        {
            InitializeConsole();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// initialize all console settings
        /// </summary>
        private void InitializeConsole()
        {
            ConsoleUtil.WindowTitle = "Traveling Car Salesperson";
            ConsoleUtil.HeaderText = "The Traveling Car Salesperson Application";

            ConsoleUtil.BodyBackgroundColor = ConsoleColor.White;
            ConsoleUtil.BodyForegroundColor = ConsoleColor.Black;

            ConsoleUtil.HeaderBackgroundColor = ConsoleColor.Blue;
            ConsoleUtil.HeaderForegroundColor = ConsoleColor.White;
            ConsoleUtil.WindowHeight = 30;
            ConsoleUtil.DisplayReset();
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        public void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayMessage("Press any key to continue.");
            ConsoleKeyInfo response = Console.ReadKey();

            ConsoleUtil.DisplayMessage("");

            Console.CursorVisible = true;
        }

        /// <summary>
        /// display the Exit prompt on a clean screen
        /// </summary>
        public void DisplayExitPrompt()
        {
            ConsoleUtil.DisplayReset();

            Console.CursorVisible = false;

            ConsoleUtil.DisplayMessage("");
            ConsoleUtil.DisplayMessage("Thank you for using the application. Press any key to Exit.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }


        /// <summary>
        /// display the welcome screen
        /// </summary>
        public void DisplayWelcomeScreen()
        {
            StringBuilder sb = new StringBuilder();
            
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Written by Nathaniel Carson");
            ConsoleUtil.DisplayMessage("Northwestern Michigan College");
            ConsoleUtil.DisplayMessage("");

            sb.Clear();
            sb.AppendFormat("You are a traveling car salesperson. ");
            sb.AppendFormat("You travel around the country looking for cars to flip and ");
            sb.AppendFormat("people to buy them. ");
            sb.AppendFormat("This application will help facilitate tracking your sales and purchases. ");
            ConsoleUtil.DisplayMessage(sb.ToString());
            ConsoleUtil.DisplayMessage("");

            sb.Clear();
            sb.AppendFormat("Your first task will be to set up your account details.");
            ConsoleUtil.DisplayMessage(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// setup the new salesperson object with the initial data
        /// Note: To maintain the pattern of only the Controller changing the data this method should
        ///       return a Salesperson object with the initial data to the controller. For simplicity in 
        ///       this demo, the ConsoleView object is allowed to access the Salesperson object's properties.
        /// </summary>
        public Salesperson DisplaySetupAccount()
        {
            //
            // initialize variables
            //
            bool validResponse;
            int userAge;
            int productAmount;
            Product.ProductType productType;
            Salesperson salesperson = new Salesperson();

            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Account Setup";
            ConsoleUtil.DisplayReset();

            //
            // get new account info
            //
            ConsoleUtil.DisplayPromptMessage("First name: ");
            salesperson.FirstName = Console.ReadLine();
            Console.WriteLine("");

            ConsoleUtil.DisplayPromptMessage("Last name: ");
            salesperson.LastName = Console.ReadLine();
            Console.WriteLine("");

            ConsoleUtil.DisplayMessage("Age");
            validResponse = ConsoleValidator.TryGetIntegerFromUser(10, 90, MAXIMUM_ATTEMPTS, "years old", out userAge);
            if (validResponse) salesperson.Age = userAge;

            ConsoleUtil.DisplayPromptMessage("Account ID: ");
            salesperson.AccountID = Console.ReadLine();
            Console.WriteLine("");

            //
            // validate user input
            //
            ConsoleUtil.DisplayMessage("Setting up inventory..");
            ConsoleUtil.DisplayMessage("");
            if (!ConsoleValidator.GetEnumValueFromUser<Product.ProductType>(MAXIMUM_ATTEMPTS, "Body Style:", out productType))
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded, returning to main menu.");
            

            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT,MAXIMUM_BUYSELL_AMOUNT,MAXIMUM_ATTEMPTS, $"{productType.ToString()}s", out productAmount)) {
                ConsoleUtil.DisplayMessage("Max attempts exceeded, returning to main menu with default value of 0 vehicles.");
            }

            //
            // set salesman product object 
            //
            salesperson.CurrentStock[productType].NumberOfUnits=productAmount;

            salesperson.Rank = Salesperson.Ranks.Beginner;

            DisplayContinuePrompt();
            return salesperson;
        }
        /// <summary>
        /// Edits the salesperson's account information
        /// </summary>
        /// <param name="salesperson"></param>
        public void DisplayEditAccountInfo(Salesperson salesperson)
        {
            bool validResponse = false;
            int userAge = 0;
            string userResponse;
            Salesperson.Ranks rank;
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Account Editing";
            ConsoleUtil.DisplayReset();

            //
            // get new account info
            //
            ConsoleUtil.DisplayMessage("Just press enter if you wish to skip the current field");
            ConsoleUtil.DisplayMessage("");

            ConsoleUtil.DisplayMessage($"First Name: {salesperson.FirstName}");

            userResponse = Console.ReadLine();
            if (userResponse != "")
                salesperson.FirstName = userResponse;

            Console.WriteLine("");

            ConsoleUtil.DisplayMessage($"Last Name: {salesperson.LastName}");

            userResponse = Console.ReadLine();
            if (userResponse != "")
                salesperson.LastName = userResponse;

            Console.WriteLine("");


            ConsoleUtil.DisplayMessage($"Age: {salesperson.Age}");
            
            validResponse = ConsoleValidator.TryGetIntegerFromUser(10, 90, MAXIMUM_ATTEMPTS, "years old", out userAge,true);
            if (validResponse) salesperson.Age = userAge;

            ConsoleUtil.DisplayMessage($"Account ID: {salesperson.AccountID}");

            if (userResponse != "")
                salesperson.AccountID = userResponse;
            Console.WriteLine("");


            ConsoleUtil.DisplayMessage($"Rank: {salesperson.Rank}");

            if (ConsoleValidator.GetEnumValueFromUser<Salesperson.Ranks>(MAXIMUM_ATTEMPTS, "Rank:", out rank,true))
                salesperson.Rank = rank;

            DisplayContinuePrompt();
        }
        /// <summary>
        /// display a closing screen when the user quits the application
        /// </summary>
        public void DisplayClosingScreen()
        {
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Thank you for using The Traveling Salesperson Application.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// get the menu choice from the user
        /// </summary>
        public MenuOption DisplayGetUserMenuChoice()
        {
            MenuOption userMenuChoice = MenuOption.None;
            bool usingMenu = true;

            while (usingMenu)
            {
                //
                // set up display area
                //
                ConsoleUtil.HeaderText = "Main Menu";
                ConsoleUtil.DisplayReset();
                Console.CursorVisible = false;

                //
                // display the menu
                //
                ConsoleUtil.DisplayMessage("Please type the number of your menu choice.");
                ConsoleUtil.DisplayMessage("");
                Console.Write(
                    "\t" + "1. Setup Account" + Environment.NewLine +
                    "\t" + "2. Travel" + Environment.NewLine +
                    "\t" + "3. Buy" + Environment.NewLine +
                    "\t" + "4. Sell" + Environment.NewLine +
                    "\t" + "5. Display Inventory" + Environment.NewLine +
                    "\t" + "6. Display Cities" + Environment.NewLine +
                    "\t" + "7. Display Account Info" + Environment.NewLine +
                    "\t" + "8. Edit Account Info" + Environment.NewLine +
                    "\t" + "9. Visit Company" + Environment.NewLine +
                    "\t" + "Q. Save Account Info" + Environment.NewLine +
                    "\t" + "W. Load Account Info" + Environment.NewLine +
                    "\t" + "E. Exit" + Environment.NewLine);

                //
                // get and process the user's response
                // note: ReadKey argument set to "true" disables the echoing of the key press
                //
                ConsoleKeyInfo userResponse = Console.ReadKey(true);
                switch (userResponse.KeyChar)
                {
                    case '1':
                        userMenuChoice = MenuOption.SetupAccount;
                        usingMenu = false;
                        break;
                    case '2':
                        userMenuChoice = MenuOption.Travel;
                        usingMenu = false;
                        break;
                    case '3':
                        userMenuChoice = MenuOption.Buy;
                        usingMenu = false;
                        break;
                    case '4':
                        userMenuChoice = MenuOption.Sell;
                        usingMenu = false;
                        break;
                    case '5':
                        userMenuChoice = MenuOption.DisplayInventory;
                        usingMenu = false;
                        break;
                    case '6':
                        userMenuChoice = MenuOption.DisplayCities;
                        usingMenu = false;
                        break;
                    case '7':
                        userMenuChoice = MenuOption.DisplayAccountInfo;
                        usingMenu = false;
                        break;
                    case '8':
                        userMenuChoice = MenuOption.EditAccountInfo;
                        usingMenu = false;
                        break;
                    case '9':
                        userMenuChoice = MenuOption.VisitCompany;
                        usingMenu = false;
                        break;
                    case 'q':
                        userMenuChoice = MenuOption.SaveAccountInfo;
                        usingMenu = false;
                        break;
                    case 'w':
                        userMenuChoice = MenuOption.LoadAccountInfo;
                        usingMenu = false;
                        break;
                    case 'E':
                    case 'e':
                        userMenuChoice = MenuOption.Exit;
                        usingMenu = false;
                        break;
                    default:
                        ConsoleUtil.DisplayMessage(
                            "It appears you have selected an incorrect choice." + Environment.NewLine +
                            "Press any key to continue or the ESC key to quit the application.");

                        userResponse = Console.ReadKey(true);
                        if (userResponse.Key == ConsoleKey.Escape)
                        {
                            usingMenu = false;
                        }
                        break;
                }
            }
            Console.CursorVisible = true;

            return userMenuChoice;
        }

        /// <summary>
        /// get the next city to travel to from the user
        /// </summary>
        /// <returns>string City</returns>
        public string DisplayGetNextCity()
        {
            //
            // initialize variables
            //
            string nextCity = "";

            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Input City";
            ConsoleUtil.DisplayReset();

            //
            // get user input
            //
            ConsoleUtil.DisplayPromptMessage("City:");
            nextCity = Console.ReadLine();

            DisplayContinuePrompt();
            return nextCity;
        }

        /// <summary>
        /// display a list of the cities traveled
        /// </summary>
        public void DisplayCitiesTraveled(Salesperson salesperson)
        {
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Cities Traveled";
            ConsoleUtil.DisplayReset();

            //
            // go through the list, and display each city
            //
            foreach (City city in salesperson.CitiesVisited)
            {
                ConsoleUtil.DisplayMessage(city.Name);
                if (city.Departed())
                    ConsoleUtil.DisplayMessage($"Spent {city.DaysSpent()} days");
                if (city.ProductsBought > 0)
                    ConsoleUtil.DisplayMessage($"Bought {city.ProductsBought} units");
                if (city.ProductsSold > 0)
                    ConsoleUtil.DisplayMessage($"Sold {city.ProductsSold} units");
                if (city.CompaniesVisited.Count > 0)
                    ConsoleUtil.DisplayMessage("\nVisited Companies: ");

                foreach (string company in city.CompaniesVisited)
                {
                    ConsoleUtil.DisplayMessage("\t" + company);
                }

                Console.WriteLine();
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// save the current account info
        /// </summary>
        public void DisplaySaveAccountInfo(string displayMessage)
        {
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Saving Account Info";
            ConsoleUtil.DisplayReset();
            
            ConsoleUtil.DisplayMessage(displayMessage);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// load the current account info
        /// </summary>
        public void DisplayLoadAccountInfo(string displayMessage)
        {
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Loading Account Info";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage(displayMessage);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the current account information
        /// </summary>
        public void DisplayAccountInfo(Salesperson salesperson)
        {
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Account Information";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage($"First Name: {ConsoleUtil.UppercaseFirst(salesperson.FirstName)}");
            ConsoleUtil.DisplayMessage($"Last Name: {ConsoleUtil.UppercaseFirst(salesperson.LastName)}");
            ConsoleUtil.DisplayMessage($"Age: {salesperson.Age}");
            ConsoleUtil.DisplayMessage($"Account ID: {salesperson.AccountID}");
            ConsoleUtil.DisplayMessage($"Rank: {salesperson.Rank}");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Displays a notification warning about the amount of products being put on backorder
        /// </summary>
        /// <param name="product"></param>
        /// <param name="numberOfUnitsSold"></param>
        public void DisplayBackorderNotification(Product product)
        {
            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Backorder Notification";
            ConsoleUtil.DisplayReset();
            
            ConsoleUtil.DisplayMessage($"{-product.NumberOfUnits} {product.Type.ToString()}s have been put on back order");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Gets an integer from the user about the number of units they want to buy
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int DisplayGetNumberOfUnitsToBuy(out Product.ProductType productType)
        {
            //
            // initialize variable
            //
            int productAmount = 0;

            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Unit Ordering";
            ConsoleUtil.DisplayReset();

            //
            // get the product type
            //
            if (!ConsoleValidator.GetEnumValueFromUser<Product.ProductType>(MAXIMUM_ATTEMPTS, "What type of vehicle do you want to buy?", out productType))
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded, returning to main menu.");

            //
            // validate the user input
            //
            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, $"{productType.ToString()}s", out productAmount))
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded, returning to main menu.");
            
            DisplayContinuePrompt();
            return productAmount;
        }


        /// <summary>
        /// Gets an integer from the user about the number of units they want to sell
        /// Sets the backorder flag if they sell more than the current inventory
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int DisplayGetNumberOfUnitsToSell(out Product.ProductType productType)
        {
            //
            // initialize variable
            //
            int productAmount = 0;

            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Unit Selling";
            ConsoleUtil.DisplayReset();


            //
            // get the product type
            //
            if (!ConsoleValidator.GetEnumValueFromUser<Product.ProductType>(MAXIMUM_ATTEMPTS, "What type of vehicle do you want to sell?", out productType))
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded, returning to main menu.");

            //
            // validate the input
            //
            if (!ConsoleValidator.TryGetIntegerFromUser(MINIMUM_BUYSELL_AMOUNT, MAXIMUM_BUYSELL_AMOUNT, MAXIMUM_ATTEMPTS, $"{productType.ToString()}s", out productAmount))
                ConsoleUtil.DisplayMessage("Maximum attempts exceeded, returning to main menu.");

            DisplayContinuePrompt();
            return productAmount;
        }



        /// <summary>
        /// Gets a company name from the user
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public string DisplayGetCompanyVisited()
        {
            //
            // initialize variable
            //
            string companyName;

            //
            // set up the console
            //
            ConsoleUtil.HeaderText = "Visiting Company";
            ConsoleUtil.DisplayReset();

            //
            // get the company name
            //
            ConsoleUtil.DisplayPromptMessage("What company did you visit? ");
            companyName = Console.ReadLine();

            DisplayContinuePrompt();
            return companyName;
        }


        /// <summary>
        /// Displays the type, amount, and backorder flag of the product passed as an argument
        /// </summary>
        /// <param name="product"></param>
        public void DisplayInventory(Dictionary<Product.ProductType, Product> products)
        {
            //
            // set up console
            //
            ConsoleUtil.HeaderText = "Current Inventory";
            ConsoleUtil.DisplayReset();

            //
            // display product
            //
            foreach (var product in products)
            {
                ConsoleUtil.DisplayMessage($"Product type : {product.Value.Color} {product.Key.ToString()}");
                ConsoleUtil.DisplayMessage($"Product model : {product.Value.ModelName}  at {product.Value.Price.ToString("C2")}");
                ConsoleUtil.DisplayMessage($"Product amount : {product.Value.NumberOfUnits}");
                ConsoleUtil.DisplayMessage($"Product is " + (product.Value.OnBackOrder ? "" : "not ") + "on back order");
                Console.WriteLine();
            }

            DisplayContinuePrompt();
        }

        #endregion
    }
}
