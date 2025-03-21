Feature: Checkout API - Calculate Total Bill

# JIRA Test Case: Checkout API - Calculate Total Bill
#    Test Case ID: TC-001
#    Test Summary: Verify that the API correctly calculates the total bill for a group order.

  Scenario: Verify that the API correctly calculates the total bill for a group order
    Given a group of 4 places an order
    When they order 4 starters, 4 mains, and 4 drinks
    Then the API should return the correct total with a 10% service charge
