Feature: Checkout API - Merging Separate Orders in a Group

# JIRA Test Case: Checkout API - Merging Separate Orders in a Group
#    Test Case ID: TC-007
#    Test Summary: Verify whether separate orders in a group are merged or handled individually.

  Scenario: Handle separate orders in the same group
    Given two people place an order
    When they order 2 mains and 2 drinks
    Then the API should calculate the total separately
    When two more people place a separate order within the same group
    Then the API should clarify whether the new order is merged or handled separately
