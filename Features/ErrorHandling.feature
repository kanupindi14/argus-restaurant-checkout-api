Feature: Checkout API - Handle Zero Quantity Order

# JIRA Test Case: Checkout API - Handle Zero Quantity Order
#    Test Case ID: TC-004
#    Test Summary: Verify that the API returns an error when all ordered items have zero quantity.

  Scenario: Reject zero quantity order
    Given a group of 3 places an order
    When they order 0 starters, 0 mains, and 0 drink
    Then the API should return an error message "Invalid order"

# JIRA Test Case: Checkout API - Handle Negative Item Quantities
#    Test Case ID: TC-005
#    Test Summary: Verify that the API does not accept negative quantities.

  Scenario: Reject negative quantities in order
    Given a group of 2 places an order
    When they order -1 starters, 2 mains, and 1 drink
    Then the API should return an error message "Invalid quantity"

