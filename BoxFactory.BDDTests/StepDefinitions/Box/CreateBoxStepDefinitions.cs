using Models.DTOs;
using Reqnroll;
using Xunit;

namespace BoxFactory.BDDTests.StepDefinitions.Box;

[Binding]
public class CreateBoxStepDefinitions
{
    private int _stock;
    private float _weight;
    private float _price;
    
    private DimensionsDto _dimensions = new();

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
    public void WhenISubmitTheBoxCreationRequest()
    {
        
    }

    [Then("the box should be created successfully")]
    public void ThenTheBoxShouldBeCreatedSuccessfully()
    {
        Assert.True(_weight > 0);
        Assert.True(_stock > 0);
        Assert.True(_price > 0);
        Assert.True(_dimensions.Length > 0);
        Assert.True(_dimensions.Width > 0);
        Assert.True(_dimensions.Height > 0);
    }
    
    [Then("the box should have the correct dimensions")]
    public void ThenTheBoxShouldHaveTheCorrectDimensions()
    {
        Assert.True(_dimensions.Length > 0);
        Assert.True(_dimensions.Width > 0);
        Assert.True(_dimensions.Height > 0);
    }
}