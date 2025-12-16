using Models.DTOs;
using TestUtils;

namespace BoxFactory.IntegrationTests.Data;

public class UpdateBoxTestData : TheoryData<BoxUpdateDto>
{
    public UpdateBoxTestData()
    {
        var valid = ModelUtils.ValidBoxUpdateDto();

        var invalidColor = ModelUtils.Clone(valid); invalidColor.Color = ""; Add(invalidColor);
        var invalidPrice = ModelUtils.Clone(valid); invalidPrice.Price = -10.0f; Add(invalidPrice);
        var invalidLength = ModelUtils.Clone(valid); invalidLength.DimensionsDto.Length = 0; Add(invalidLength);
        var invalidWidth = ModelUtils.Clone(valid); invalidWidth.DimensionsDto.Width = -5; Add(invalidWidth);
        var invalidHeight = ModelUtils.Clone(valid); invalidHeight.DimensionsDto.Height = 0; Add(invalidHeight);
        var invalidStock = ModelUtils.Clone(valid); invalidStock.Stock = -1; Add(invalidStock);
        var invalidWeight = ModelUtils.Clone(valid); invalidWeight.Weight = -2; Add(invalidWeight);
        var invalidMaterial = ModelUtils.Clone(valid); invalidMaterial.Material = ""; Add(invalidMaterial);
    }
}