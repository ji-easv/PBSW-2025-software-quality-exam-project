---
title: Software Quality Exam
author: Júlia Ilášová
theme:
  name: gruvbox-dark
---

```csharp +line_numbers
public async Task<IEnumerable<Box>> GetBoxesForOderAsync(Dictionary<Guid, int> boxQuantities)
{
    var boxIds = boxQuantities.Keys;
    var boxes = (await boxRepository.GetBoxesByIdsAsync(boxIds)).ToList();

    // Check if all boxes exist
    if (boxes.Count != boxIds.Count)
    {
        var foundBoxIds = boxes.Select(b => b.Id);
        var missingBoxIds = boxIds.Except(foundBoxIds);
        throw new NotFoundException($"Boxes with ids {string.Join(", ", missingBoxIds)} not found");
    }

    // Check if each box is in the required stock
    foreach (var box in boxes)
    {
        var requiredQuantity = boxQuantities[box.Id];
        if (box.Stock < requiredQuantity)
            throw new ValidationException(
                $"Box with id {box.Id} does not have enough stock. Required: {requiredQuantity}, Available: {box.Stock}");
    }

    return boxes;
}
```

<!-- end_slide -->

```mermaid +render
stateDiagram
    state "1-7" as Input
    state "13-15" as ForEachStart
    state "16-18" as SecondIf
    state "22-23" as Sink

    Input --> Sink : Some boxes not found (NotFoundException)
    Input --> ForEachStart : All boxes found
    ForEachStart --> SecondIf : Check stock for each box
    SecondIf --> Sink : Not enough in stock (ValidationException)
    SecondIf --> ForEachStart
    ForEachStart --> Sink : All boxes checked -> Return boxes
```