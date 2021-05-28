Feature: Lists

Scenario: Archive All Cards in a List
	Given I a have an existing board
	And I have an existing list
	When I archive all cards in the list
	Then all cards are archived

Scenario: Archive All Cards in a Board
	Given I a have an existing board
	And I have an existing list
	When I archive all cards in the board
	Then all cards are archived in the board