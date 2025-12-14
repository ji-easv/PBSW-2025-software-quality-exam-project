using Core.Services.Interfaces;
using Infrastructure.Interfaces;
using Models.DTOs;
using Models.Models;
using Models.Util;

namespace BoxFactoryAPI;

public class DbInitialize(
    IOrderService orderService,
    IOrderRepository orderRepository,
    IBoxService boxService,
    ICustomerService customerService)
{
    private readonly Random _rnd = new Random();

    public async Task InitializeData()
    {
        var boxes = await CreateBoxesAsync();
        var customerEmails = await CreateCustomersAsync();
        
        var currentDate = DateTime.UtcNow;
        for (var i = 0; i < 12; i++)
        {
            var numberOfOrders = _rnd.Next(5, 15);
            for (var j = 0; j < numberOfOrders; j++)
            {
                var numberOfBoxesInOrder = _rnd.Next(1, 3);
                var saveBoxes = new Dictionary<Guid, int>();
                for (var k = 0; k < numberOfBoxesInOrder; k++)
                {
                    var selectBox = boxes[_rnd.Next(boxes.Count)].Id;
                    saveBoxes.TryAdd(selectBox, _rnd.Next(1, 10));
                }

                var createOrder = new OrderCreateDto
                {
                    Boxes = saveBoxes,
                    CustomerEmail = customerEmails.ElementAt(_rnd.Next(customerEmails.Count)),
                };
                var order = await orderService.CreateAsync(createOrder);
                order.CreatedAt = currentDate.AddMonths(-i);
                order.UpdatedAt = currentDate.AddMonths(-i);
                await orderRepository.UpdateOrderAsync(order);
            }
        }
    }

    private async Task<HashSet<string>> CreateCustomersAsync()
    {
        var firstNames = new List<string>
            { "John", "Jane", "Jim", "Jenny", "James", "Judy", "Joe", "Jessica", "Jack", "Julia" };

        var lastNames = new List<string>
            { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson" };

        var cities = new List<string>
        {
            "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego",
            "Dallas", "San Jose"
        };

        var streetNames = new List<string>
        {
            "Main St", "Oak St", "Pine St", "Maple St", "Cedar St", "Elm St", "Washington St", "Lake St", "Hill St",
            "Sunset St"
        };

        var countries = new List<string>
            { "USA", "Canada", "Mexico", "UK", "France", "Germany", "Netherlands", "Belgium", "Italy", "Spain" };

        var postalCodes = new List<string>
            { "10001", "90001", "60601", "77001", "85001", "19101", "78201", "92101", "75201", "95101" };

        var houseNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var houseNumberAdditions = new List<string?>
            { "A", "B", "C", "D", null };

        var customerEmails = new HashSet<string>();
        for (var i = 0; i < 10; i++)
        {
            var firstName = firstNames[_rnd.Next(firstNames.Count)];
            var lastName = lastNames[_rnd.Next(lastNames.Count)];
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com";
            if (!customerEmails.Add(email))
            {
                i--;
                continue;
            }

            var createCustomerDto = new CreateCustomerDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                CreateAddressDto = new CreateAddressDto
                {
                    StreetName = streetNames[_rnd.Next(streetNames.Count)],
                    HouseNumber = houseNumbers[_rnd.Next(houseNumbers.Count)],
                    HouseNumberAddition = houseNumberAdditions[_rnd.Next(houseNumberAdditions.Count)],
                    City = cities[_rnd.Next(cities.Count)],
                    Country = countries[_rnd.Next(countries.Count)],
                    PostalCode = postalCodes[_rnd.Next(postalCodes.Count)]
                }
            };

            await customerService.CreateCustomerAsync(createCustomerDto);
        }

        return customerEmails;
    }

    private async Task<List<Box>> CreateBoxesAsync()
    {
        var numberOfBoxes = _rnd.Next(20, 30);
        var materials = new List<string> { "cardboard", "plastic", "wood", "metal" };

        var colors = new List<string>
        {
            "red", "blue", "green", "yellow", "black",
            "white", "brown", "grey", "orange", "purple",
            "pink", "gold", "silver", "bronze", "copper"
        };
        for (var i = 0; i < numberOfBoxes; i++)
        {
            var randomMaterial = materials[_rnd.Next(materials.Count)];
            var randomColor = colors[_rnd.Next(colors.Count)];

            await boxService.CreateBoxAsync(new BoxCreateDto
            {
                Color = randomColor,
                Material = randomMaterial,
                Price = _rnd.NextSingle() * 100,
                Stock = _rnd.Next(100, 10000),
                DimensionsDto = new DimensionsDto
                {
                    Height = _rnd.Next(1, 100),
                    Length = _rnd.Next(1, 100),
                    Width = _rnd.Next(1, 100)
                },
                Weight = _rnd.NextSingle() * 100
            });
        }

        var boxParameter = new BoxParameters
        {
            BoxesPerPage = 2000,
            CurrentPage = 1,
            SearchTerm = "",
            Descending = false
        };

        return ((await boxService.SearchBoxesAsync(boxParameter)).Boxes ?? []).ToList();
    }
}