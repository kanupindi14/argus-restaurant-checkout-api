Feature: Checkout API - Modify Order

#  JIRA Test Case: Checkout API - Modify Order
#    Test Case ID: TC-003
#    Test Summary: Verify that the total bill updates correctly when an order is modified.

  Scenario: Modify order after cancellation
    Given a group of 4 places an order
    When they place an order 4 starters, 4 mains, and 4 drinks_
    Then the API should return the correct total
    When one member cancels their order
    Then the API should recalculate the total correctly and adjust the service charge accordingly
