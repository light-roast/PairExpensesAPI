
# PairXpenses API - Backend project for Codigo facilito Bootcamp on web development with .NET.

## Table of Contents

- [Overview](#overview)

   - [Summary](#summary)

- [Author](#author)

## Overview

### Summary

Develop an API using .NET, EF, and SQLite to manage the data handling of the .NET full-stack application called PairXpenses. The application is designed to be used by couples (partners, roommates, or spouses) to keep track of expenses that the couple is supposed to make monthly on items such as rent, utilities, groceries, among others. As the daily dynamics of the mentioned couples unfold, it is highly likely that one of the members ends up making more payments than the other during the month. This application allows establishing a monthly expense allocation protocol: from the classic 50/50 split to any other proportion of expenses that each couple feels comfortable establishing according to their monthly incomes and personal agreements. With a simple click, the application provides a report of monthly expenses as well as which member of the couple owes the other to settle the month and start the accounting for the next one. In addition to payments, the app also allows users to record debts that one user has with the other. Debts are a separate entity because they are not intended to be shared expenses but rather payments that one member made for the other and commits to repay at the end of the month.

This API has three main entities: User, Payment, and Debt. The API generates an SQLite database through migration called Database.db. The current database is gitignored, so to download the project and use it, you need to create a new migration to generate the database with the four users and default passwords, which must then be changed for normal use of the app. Likewise, the CORS configuration's WithOrigins in program.cs must be set up to receive the front-end app's localhost that will be using the API (You can use the PairXpenses front-end application created in Blazor in conjunction with this API as a reference). The API implements the necessary services and controllers to CRUD (Create, Read, Update, Delete) all the data in the database and provides methods to generate real-time reports of the monthly expenses for the couple using the app. 

### Project Requirements (as stated by Codigo facilito):

- Implement Auth using JWT.
- Connect to a DB.
- Implement serilog log creation.
- Utilize version control with Git and host the repository on GitHub.
- Write a README (using Markdown) that documents your project, including:
   - The project's purpose.
   - Technologies used.
   - Features.
   - Plans for future development.

#### Features

- Uses local Database.db with SQlite.
- Implement authentication and authorization and gives a JWT token to the front using the API.
- Allow 2 different couples to use the database with no interference of thier data.
- The API is deployed on azure to be used with the front-end app listed above.

### Links

- Solution URL: [GitHub Repository](https://github.com/light-roast/PairExpensesAPI)
- Azure URL: [Deployed on azure](https://pairxpenses.azurewebsites.net/)

## Author

- Website: [Daniel Echeverri Llano](https://daniel-echeverri-portfolio.netlify.app/)
- Frontend Mentor: [@light-roast](https://www.frontendmentor.io/profile/light-roast)
