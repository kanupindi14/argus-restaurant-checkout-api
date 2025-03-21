Feature: Checkout API - Apply Drink Discount

# JIRA Test Case: Checkout API - Apply Drink Discount
#    Test Case ID: TC-002
#    Test Summary: Verify that the API correctly applies a 30% discount on drinks ordered before 19:00.
  Scenario: Apply 30% discount for drinks ordered before 19:00
    Given a group of 2 places an order at "18:30"
    When they order 1 starter, 2 mains, and 2 drinks
    Then the API should apply a 30% discount on the drinks

  Scenario: Add late joiners after 19:00
    Given two more people join the group at "20:00"
    And they order 2 mains and 2 drinks
    Then the API should calculate the final bill correctly, applying the discount only to drinks ordered before 19:00
