1. Execute RUNTHIS.exe
2. Verify OrderListTarget.xlsx for orders 1-2 hours approx
3. Wait script to complete orders
4. Continous beeping means the script is ready for adding manual orders and checkout.
5. After completion check ManualAdd.csv for card not included in the order
	NO RESULTS
		- No results or encountered an error.  Double check card if can be added manually
	OUT OF STOCK
		- Card is out of stock
	PRICE IS NOW INVALID. PLEASE UPDATE PRICE
		- Price had already changed from input.
		- Update card price in OrderListTarget.xlsx 
	INVALID QUALITY
		- Quality for the card is not available.
		- Manually update
6. Backup ManualAdd.csv for checking and delete generated file.
7. Close NUnit runner window upon completion.