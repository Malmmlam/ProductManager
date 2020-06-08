using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inlämning4.Repositories;


namespace Inlämning4.Classes
{
    class UI
    {
        private static string _enter = "Press enter to continue.";
        private static FileProductRepository _products = new FileProductRepository();
        private static FileStoreRepository _stores = new FileStoreRepository();
        private static FileManufacturerRepository _manufacturers = new FileManufacturerRepository();

        //Menu lists----------------------------------------------------------------------------------------------------
        private static List<string> _mainMenuOptions = new List<string>()
        {
            "1. Product Options",
            "2. Store Options",
            "3. Manufacturer Options",
            "0. Exit"
        };

        private static List<string> _productMenuOptions = new List<string>()
        {
            "1. Create Product",
            "2. Delete Product",
            "3. List all Products",
            "4. Search for Product",
            "0. Go Back"
        };

        private static List<string> _productSearchOptions = new List<string>()
        {
            "1. Search by product name",
            "2. Search by maximum price"
        };

        private static List<string> _storeMenuOptions = new List<string>()
        {
            "1. Create Store",
            "2. Delete Store",
            "3. List all Stores",
            "4. View Store Stock",
            "5. Add Stock to Store",
            "6. Remove Stock from Store",
            "0. Go Back"
        };


        private static List<string> _manufacturerMenuOptions = new List<string>()
        {
            "1. Add Manufacturer",
            "2. Delete Manufacturer",
            "3. List all manufacturers",
            "0. Go Back"
        };

        //General methods for writing menus and handling menu input-----------------------------------------------------
        public static void WriteMenu(List<string> menu)
        {
            Console.WriteLine("Please make a selection using the number keys.");
            foreach (var option in menu)
            {
                Console.WriteLine(option);
            }
        }

        private static int GetUserInputInt(int maxInt, int minInt)
        {
            while (true)
            {
                var userInput = Console.ReadLine();

                try
                {
                    var inputInt = int.Parse(userInput);

                    if (inputInt > maxInt || inputInt < minInt)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    return inputInt;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message + $" Only numbers above {minInt} and up to {maxInt} are valid.");
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message + " Entry cannot be blank");
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message + " Only numbers are valid.");
                }
            }
        }

        private static decimal GetUserInputDecimal()
        {
            while (true)
            {
                var userInput = Console.ReadLine();
                try
                {
                    var inputDecimal = decimal.Parse(userInput);

                    if (inputDecimal < 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    return inputDecimal;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.Message + "Price cannot be less than 0");
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e.Message + " Price field cannot be empty");
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message + " Price can only include numbers and ','.");
                }
            }
        }

        private static string GetUserInputString()
        {
            while (true)
            {
                var userInput = Console.ReadLine();
                try
                {
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        throw new ArgumentNullException();
                    }

                    return userInput;
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("String cannot be empty. Please try again.");
                }
            }
        }

        private static void Enter()
        {
            Console.WriteLine(_enter);
            Console.ReadLine();
            MainMenuInterface();
        }

        private static void ListItems(IEnumerable<object> listToList)
        {
            var i = 1;
            foreach (var item in listToList)
            {
                Console.WriteLine(i + ". " + item);
                i++;
            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Exiting...");

            _products.Save(_products.ProductJsonPath, _products.ProductList);
            _manufacturers.Save(_manufacturers.ManufacturerJsonPath,
                _manufacturers.ManufacturersList);
            _stores.Save(_stores.StoreJsonPath, _stores.StoreList);

            Environment.Exit(0);
        }

        private static bool Exists(IEnumerable<Object> fileRepository)
        {
            if (fileRepository.Count() != 0) return true;

            Console.WriteLine("Nothing on file");
            return false;
        }

        //Handling input in ProductMenu---------------------------------------------------------------------------------
        private static void AddProduct()
        {
            Console.WriteLine("Please enter a product name.");
            var userInput = GetUserInputString();

            Console.WriteLine("Please enter a product price.");
            var inputDecimal = GetUserInputDecimal();

            var inputInt = 0;

            if (_manufacturers.ManufacturersList.Count > 0)
            {
                Console.WriteLine(
                    "Please choose a manufacturer from the list below or type 0 to add a new manufacturer.");
                ListItems(_manufacturers.ManufacturersList);
                inputInt = GetUserInputInt(_manufacturers.ManufacturersList.Count, 0);
            }
            else if (_manufacturers.ManufacturersList.Count == 0)
            {
                Console.WriteLine("No manufacturers on file. You can create one now.");
            }

            if (inputInt == 0)
            {
                AddManufacturerProductMenu();

                _products.Insert(userInput, inputDecimal,
                    _manufacturers.ManufacturersList.Last());
            }
            else
            {
                _products.Insert(userInput, inputDecimal,
                    _manufacturers.ManufacturersList[inputInt - 1]);
            }


            Console.WriteLine($"Product Added \n{_products.GetAll().Last()}");
            _manufacturers.Save(_manufacturers.ManufacturerJsonPath,
                _manufacturers.ManufacturersList);
            _products.Save(_products.ProductJsonPath, _products.ProductList);
            Enter();
        }

        private static void AddManufacturerProductMenu()
        {
            Console.WriteLine("Please enter a manufacturer name.");
            var manufacturerName = GetUserInputString();

            Console.WriteLine("Please enter a manufacturer location");
            var manufacturerLocation = GetUserInputString();

            _manufacturers.Insert(manufacturerName, manufacturerLocation);

            Console.WriteLine($"Manufacturer Added \n{_manufacturers.GetAll().Last()}");
        }


        private static void DeleteProduct()
        {
            var checkForProducts = Exists(_products.ProductList);

            if (checkForProducts)
            {
                var matchList = SearchByNameProduct();

                Console.WriteLine("Please select product to delete with number keys");

                var inputInt = GetUserInputInt(_products.ProductList.Count, 1);

                _stores.RemoveProductFromStore(matchList[inputInt - 1]);


                _products.Delete(
                    _products.ProductList.Find(product => product.Equals(matchList[inputInt - 1])));

                Console.WriteLine("Product Deleted");
            }

            Enter();
        }

        private static void ListAllProducts()
        {
            var checkForProducts = Exists(_products.ProductList);

            if (checkForProducts)
            {
                Console.WriteLine($"Total number of products: {_products.GetAll().Count()}");
                ListItems(_products.GetAll());
            }

            Enter();
        }

        private static void SearchForProducts()
        {
            var checkForProducts = Exists(_products.ProductList);

            if (checkForProducts)
            {
                Console.WriteLine("Please choose how you would like to search.");
                foreach (var option in _productSearchOptions)
                {
                    Console.WriteLine(option);
                }

                var inputInt = GetUserInputInt(2, 1);

                switch (inputInt)
                {
                    case 1:
                        SearchByNameProduct();
                        Enter();
                        break;
                    case 2:
                        SearchByPrice();
                        break;
                }
            }
            else
            {
                Enter();
            }
        }

        private static List<Product> SearchByNameProduct()
        {
            Console.WriteLine("Enter name search string.");
            var searchString = GetUserInputString();

            var searchResults = _products.SearchForProductByName(searchString);
            Console.WriteLine($"Found {searchResults.Count} results:");

            if (searchResults.Count == 0)
            {
                Console.WriteLine("No matches found");
                Enter();
            }

            var i = 1;
            foreach (var product in searchResults)
            {
                Console.WriteLine(i + ". " + product);
                i++;
            }

            return searchResults;
        }

        private static void SearchByPrice()
        {
            Console.WriteLine("Please enter maximum price");
            var maxPrice = GetUserInputDecimal();
            _products.SearchForProductByPrice(maxPrice);
            Enter();
        }

        //Handling input in StoreMenu-----------------------------------------------------------------------------------

        private static void AddStore()
        {
            Console.WriteLine("Please enter a store name.");
            var storeName = GetUserInputString();

            Console.WriteLine("Please enter a store location");
            var storeLocation = GetUserInputString();

            _stores.Insert(storeName, storeLocation);
            Console.WriteLine($"Store Added \n{_stores.StoreList.Last()}");
            _stores.Save(_stores.StoreJsonPath, _stores.StoreList);

            Enter();
        }

        private static void DeleteStore()
        {
            var checkForStores = Exists(_stores.StoreList);

            if (checkForStores)
            {
                var matchList = SearchByNameStore();

                Console.WriteLine("Please select store to delete with number keys");

                var inputInt = GetUserInputInt(_stores.StoreList.Count, 1);
                _stores.Delete(
                    _stores.StoreList.Find(store => store.Equals(matchList[inputInt - 1])));

                Console.WriteLine("Store Deleted");
            }

            Enter();
        }

        private static void ListAllStores()
        {
            var checkForStores = Exists(_stores.StoreList);

            if (checkForStores)
            {
                Console.WriteLine($"Total number of stores: {_stores.StoreList.Count()}");
                ListItems(_stores.StoreList);
            }

            Enter();
        }

        private static void CheckStoreStock()
        {
            var matchList = SearchByNameStore();

            if (matchList.Count == 0)
            {
                Console.WriteLine("No stores found.");
                Enter();
            }

            Console.WriteLine("Select store to check stock at with number keys.");

            var inputInt = GetUserInputInt(_stores.StoreList.Count, 1);
            var checkForProducts = Exists(matchList[inputInt - 1].StockedProducts);

            if (checkForProducts)
            {
                Console.WriteLine("Products Stocked: ");
                ListItems(matchList[inputInt - 1].StockedProducts);
            }

            Enter();
        }


        private static List<Store> SearchByNameStore()
        {
            Console.WriteLine("Enter name search string.");
            List<Store> searchResults;

            while (true)
            {
                var searchString = GetUserInputString();
                searchResults = _stores.SearchForStoreByName(searchString);
                if (searchResults.Count == 0)
                {
                    Console.WriteLine("No matches found. Please try searching again.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"Found {searchResults.Count} stores:");

            var i = 1;
            foreach (var store in searchResults)
            {
                Console.WriteLine(i + ". " + store);
                i++;
            }

            return searchResults;
        }

        private static void AddStockToStore()
        {
            var checkForStores = Exists(_stores.StoreList);
            if (checkForStores)
            {
                var matchList = SearchByNameStore();

                Console.WriteLine("Please use the number keys to make your selection");

                var storeSelectionInt = GetUserInputInt(_stores.StoreList.Count, 1);
                var checkForProducts = Exists(_products.ProductList);

                if (checkForProducts)
                {
                    Console.WriteLine("Choose Product to add to store");
                    ListItems(_products.ProductList);
                    var productSelectionInt = GetUserInputInt(_products.ProductList.Count, 1);

                    if (matchList[storeSelectionInt - 1].StockedProducts
                        .Contains(_products.ProductList[productSelectionInt - 1]))
                    {
                        Console.WriteLine("Product already stocked at store.");
                    }
                    else
                    {
                        _stores.AddProductToStore(matchList[storeSelectionInt - 1],
                            _products.ProductList[productSelectionInt - 1]);

                        Console.WriteLine("Product Added");
                    }
                }
            }

            Enter();
        }

        private static void RemoveStockFromStore()
        {
            var checkForStores = Exists(_stores.StoreList);
            if (checkForStores)
            {
                var matchList = SearchByNameStore();
                Console.WriteLine("Please use the number keys to make your selection");

                var storeSelectionInt = GetUserInputInt(matchList.Count, 1);
                var checkForProducts = Exists(matchList[storeSelectionInt - 1].StockedProducts);

                if (checkForProducts)
                {
                    Console.WriteLine("Choose Product to remove from store");
                    ListItems(matchList[storeSelectionInt - 1].StockedProducts);
                    var productSelectionInt =
                        GetUserInputInt(matchList[storeSelectionInt - 1].StockedProducts.Count, 1);

                    _stores.RemoveProductFromStore(matchList[storeSelectionInt - 1]
                        .StockedProducts[productSelectionInt - 1]);

                    Console.WriteLine("Product Removed");
                }
            }

            Enter();
        }

        //Handling input in ManufacturerMenu----------------------------------------------------------------------------
        private static void AddManufacturer()
        {
            Console.WriteLine("Please enter a manufacturer name.");
            var manufacturerName = GetUserInputString();

            Console.WriteLine("Please enter a manufacturer location");
            var manufacturerLocation = GetUserInputString();

            _manufacturers.Insert(manufacturerName, manufacturerLocation);
            Console.WriteLine($"Manufacturer Added \n{_manufacturers.GetAll().Last()}");
            _manufacturers.Save(_manufacturers.ManufacturerJsonPath, _manufacturers.ManufacturersList);

            Enter();
        }

        private static void DeleteManufacturer()
        {
            SafetyCheck();
            var checkForManufacturers = Exists(_manufacturers.ManufacturersList);

            if (checkForManufacturers)
            {
                Console.WriteLine("Enter name of manufacturer to delete.");
                var userInput = GetUserInputString();

                var match = _manufacturers.GetAll()
                    .Where(manufacturer => manufacturer.Name.Contains(userInput));
                var matchList = match.ToList();

                if (matchList.Count == 0)
                {
                    Console.WriteLine("No manufacturers found.");
                    Enter();
                }

                Console.WriteLine("Please select manufacturer to delete with number keys");

                ListItems(matchList);

                var inputInt = GetUserInputInt(_manufacturers.ManufacturersList.Count, 1);

                _products.ProductList.RemoveAll(p =>
                    p.Manufacturer.Name == matchList[inputInt - 1].Name);
                _manufacturers.Delete(matchList[inputInt - 1]);

                Console.WriteLine("Manufacturer Deleted");
            }

            Enter();
        }

        private static void SafetyCheck()
        {
            Console.WriteLine(
                "This will delete all products produced by this manufacturer.\n Type 1 to continue or 0 to go back.");
            var inputInt = GetUserInputInt(1, 0);
            if (inputInt == 0)
            {
                MainMenuInterface();
            }
        }

        private static void ListAllManufacturers()
        {
            var checkForManufacturers = Exists(_manufacturers.ManufacturersList);

            if (checkForManufacturers)
            {
                Console.WriteLine(
                    $"Total number of manufacturers: {_manufacturers.ManufacturersList.Count()}");
                ListItems(_manufacturers.ManufacturersList);
            }

            Enter();
        }

        //Methods for handling menu interfaces--------------------------------------------------------------------------
        public static void MainMenuInterface()
        {
            WriteMenu(_mainMenuOptions);
            var inputInt = GetUserInputInt(3, 0);

            switch (inputInt)
            {
                case 1:
                    ProductMenuInterface();
                    break;
                case 2:
                    StoreMenuInterface();
                    break;
                case 3:
                    ManufacturerMenuInterface();
                    break;
                case 0:
                    ExitProgram();
                    break;
            }
        }

        public static void ProductMenuInterface()
        {
            WriteMenu(_productMenuOptions);
            var inputInt = GetUserInputInt(4, 0);

            switch (inputInt)
            {
                case 1:
                    AddProduct();
                    break;
                case 2:
                    DeleteProduct();
                    break;
                case 3:
                    ListAllProducts();
                    break;
                case 4:
                    SearchForProducts();
                    break;
                case 0:
                    MainMenuInterface();
                    break;
            }
        }

        public static void StoreMenuInterface()
        {
            WriteMenu(_storeMenuOptions);
            var inputInt = GetUserInputInt(6, 0);

            switch (inputInt)
            {
                case 1:
                    AddStore();
                    break;
                case 2:
                    DeleteStore();
                    break;
                case 3:
                    ListAllStores();
                    break;
                case 4:
                    CheckStoreStock();
                    break;
                case 5:
                    AddStockToStore();
                    break;
                case 6:
                    RemoveStockFromStore();
                    break;
                case 0:
                    MainMenuInterface();
                    break;
            }
        }


        public static void ManufacturerMenuInterface()
        {
            WriteMenu(_manufacturerMenuOptions);
            var inputInt = GetUserInputInt(3, 0);

            switch (inputInt)
            {
                case 1:
                    AddManufacturer();
                    break;
                case 2:
                    DeleteManufacturer();
                    break;
                case 3:
                    ListAllManufacturers();
                    break;
                case 0:
                    MainMenuInterface();
                    break;
            }
        }
    }
}