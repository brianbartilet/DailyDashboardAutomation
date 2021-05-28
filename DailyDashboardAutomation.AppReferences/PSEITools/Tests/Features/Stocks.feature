Feature: Stocks

Scenario: Get Stock Price
	When I fetch stock information
	Then the stock information is fetched successfully
