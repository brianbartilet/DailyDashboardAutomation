Feature: Board
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Get Board Tags
	Given I a have an existing board
	When I get all labels from the board
	Then the labels are fetched successfully