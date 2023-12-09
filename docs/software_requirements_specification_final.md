# Overview

The Software Requirements Specification (SRS) document serves as the base for our Wager Watch web app. It acts as a comprehensive guide, defining the functionalities, constraints, and outcomes of the web app. This document is the updated version of [software_requirements_specification.md](https://github.com/alexkaiser34/gvsu-cis350-sports-betting-app/blob/main/docs/software_requirements_specification.md)

# Software Requirements

Functional/Non-Functional Requirements
Name of Feature
| ID |  Requirement.

## Functional Requirements

### User Registration

| FR1 | Users shall be able to create an account by providing their email, username, and password.

| FR2 | Users shall be able to log out of their account.

| FR3 | Users shall be able to log in to existing accounts.

| FR4 | Users who are logged in shall be remembered when reopening the browser.

| FR5 | Users first and last name shall be displayed on the nabber.

### History and Home Page

| FR6 | Users shall be able to view a list of all their wagers they have placed on the history page.

| FR7 | Users shall be able to view a graph showing their net profit in the history page.

| FR8 | Users shall be able to see a list of their currently active wagers on the home page.

| FR9 | Users shall be prompted to log in on the home page if not already logged in.

| FR10 | The home page shall display application features.

### Bets Page

| FR11 | Shall display the user relevant information for each upcoming NFL game, including teams, odds, and game time.

| FR12 | Shall display buttons corresponding to different bets for upcoming NFL games.

| FR13 | The user shall be able to create a wager slip when clicking on the bet button.

| FR14 | The user shall be able to enter a bet amount and place the bet on the bets page.

| FR15 | The user shall be able to remove a bet from the wager.

| FR16 | The system shall prevent users from placing duplicate bets on the same game and event.

| FR17 | The user shall not be able to place a bet unless they are logged in.

## Non-Functional Requirements

### Security

| NFR1 | Users shall only have to enter their username and password only once when logging in on the same browser on the same device.

| NFR2 | The bet tracking system shall ensure that users can only access their own information.

| NFR3 | User passwords shall be hidden when being entered on the browser.

| NFR4 | Only upcoming games shall be allowed to have bets placed on them.

| NFR5 | The website shall be hosted using a secure protocol HTTPS.

| NFR6 | Passwords shall meet specific complexity requirements (minimum length, combination of letters, numbers, and special characters).

| NFR7 | User passwords shall be hidden when being entered on the browser.

### Performance

| NFR8 | All pages shall load in 5 seconds.

| NFR9 | The app should handle a minimum of 10 concurrent users.

| NFR10 | The database shall be running and available 24 hours a day, 7 days a week.

| NFR11 | New data from the external API shall be requested and updated every 24 hours.

| NFR12 | Database shall backup 1 time every 7 days.

### Universal Website Styling

| NFR13 | At the top right corner of each web page the user shall be able to access a button for account details.

| NFR14 | All web pages shall allow for users to select themes light and dark.

| NFR15 | All web pages shall fit to user monitor size.

| NFR16 | Home page application features hover shall conclude in .3 seconds.

| NFR17 | Data displayed on homepage shall change based on if the user is logged in to account or not logged into account.

# Software Artifacts

### Use Case Diagrams

[User Dashboard](https://github.com/alexkaiser34/gvsu-cis350-sports-betting-app/blob/main/artifacts/use_case_diagrams/User%20Dashboard.png)

[Login Interface](https://github.com/alexkaiser34/gvsu-cis350-sports-betting-app/blob/main/artifacts/use_case_diagrams/Login%20Interface.png)

[Bets Page](https://github.com/alexkaiser34/gvsu-cis350-sports-betting-app/blob/main/artifacts/use_case_diagrams/Bets%20Page.jpg)