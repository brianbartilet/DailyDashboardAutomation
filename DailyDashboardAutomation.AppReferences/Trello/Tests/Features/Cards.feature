Feature: Cards

	
Scenario: View All Cards in A Board
	Given I a have an existing board
	When I view all available cards
	Then I can view all my cards

Scenario: View Cards in a Board List
	Given I a have an existing board
	And I have an existing list
	When I view all listed cards
	Then I can view all my cards

Scenario: Create A Card
	Given I a have an existing board
	And I have an existing list
	When I add a card with the following items to the top of list
	Then my card is added successfully
	When I add a card with the following items to the bottom of list
	Then my card is added successfully

Scenario: Move A Card To Another List
	Given I a have an existing board
	And I have an existing list
	When I add a card with the following items to the top of list
	And I move the card to another list
	Then the card is moved successfully

Scenario: Archive A Card
	Given I a have an existing board
	And I have an existing list
	When I add a card with the following items to the top of list
	And I archive the card
	Then the card is archived successfully


