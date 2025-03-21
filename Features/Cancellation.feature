Feature: Checkout API - Cancel Order

# JIRA Test Case: Checkout API - Cancel Order
# Test Case ID: TC-006
# Test Summary: Verify that canceling an order removes all charges, including service charge.

  Scenario: Cancel entire order
    Given a group of 4 places an order
    When they placed order 4 starters, 4 mains, and 4 drinks.
    Then the API should return the correct total.
    When they cancel the entire order
    Then the API should return "Order canceled" and total should be 0
