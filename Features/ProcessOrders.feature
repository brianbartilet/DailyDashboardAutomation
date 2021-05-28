Feature: AbuTest
	BUGS:
		1. No support for foreign cards
		2. Comma Separation causes error
		3. Conditional Quality Support

Scenario: Process Orders
	Given I am on the order page
	And my order cart is empty
	When I export an order in a spreadsheet in the shopping cart
	Then the order would be processed



