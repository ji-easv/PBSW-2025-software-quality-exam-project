using Models.DTOs;
using Reqnroll;

namespace BoxFactory.IntegrationTests.BDD.StepDefinitions.Box;

[Binding]
public class CreateBoxStepDefinitions(HttpClient httpClient)
{
    private int _stock;
    private float _weight;
    private float _price;

    private DimensionsDto? _dimensions;
    private HttpResponseMessage? _responseMessage;
    private Models.Models.Box? _createdBox;
    
    [Given("I want to create a new box with weight {float}, stock {int}, and price {float}")]
    public void GivenIWantToCreateANewBoxWithWeightStockAndPrice(float weight, int stock, float price)
    {
        _weight = weight;
        _stock = stock;
        _price = price;
    }

    [Given("the box dimensions are length {int}, width {int}, and height {int}")]
    public void GivenTheBoxDimensionsAreLengthWidthAndHeight(int length, int width, int height)
    {
        _dimensions = new DimensionsDto
        {
            Length = length,
            Width = width,
            Height = height
        };
    }

    [When("I submit the box creation request")]
    public async Task WhenISubmitTheBoxCreationRequest()
    {
        var boxDto = new BoxCreateDto
        {
            Weight = _weight,
            Stock = _stock,
            Price = _price,
            DimensionsDto = _dimensions
        };

        _responseMessage = await httpClient.PostAsJsonAsync("/box", boxDto);
    }

    [Then("the box should be created successfully")]
    public void ThenTheBoxShouldBeCreatedSuccessfully()
    {
        Assert.NotNull(_responseMessage);
        Assert.Equal(System.Net.HttpStatusCode.OK, _responseMessage!.StatusCode);

        _createdBox = _responseMessage.Content.ReadFromJsonAsync<Models.Models.Box>().Result;
        Assert.NotNull(_createdBox);
        Assert.Equal(_weight, _createdBox!.Weight);
        Assert.Equal(_stock, _createdBox.Stock);
        Assert.Equal(_price, _createdBox.Price);
    }

    [Then("the box should have the correct dimensions")]
    public void ThenTheBoxShouldHaveTheCorrectDimensions()
    {
        Assert.NotNull(_createdBox);
        Assert.NotNull(_createdBox!.Dimensions);
        Assert.Equal(_dimensions!.Length, _createdBox.Dimensions!.Length);
        Assert.Equal(_dimensions.Width, _createdBox.Dimensions.Width);
        Assert.Equal(_dimensions.Height, _createdBox.Dimensions.Height);
    }
}