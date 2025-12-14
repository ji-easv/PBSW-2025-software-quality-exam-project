Feature: Create Box

New boxes can be added in the Box Factory system

Scenario Outline: Create a new box
	Given I want to create a new box with weight <weight>, stock <stock>, and price <price>
	And the box dimensions are length <length>, width <width>, and height <height>
	When I submit the box creation request
	Then the box should be created successfully
	And the box should have the correct dimensions

	Examples:
	  | length | width | height | weight | price | stock |
	  | 10     | 5     | 8      | 5      | 20    | 50    |
	  | 15     | 10    | 12     | 8      | 30    | 30    |
	  | 20     | 15    | 10     | 12     | 40    | 20    |