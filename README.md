---
runme:
  id: 01HRQ7J4Z481B716R0RVRZYADC
  version: v3
---

# PairXpenses API - Backend project for the Codigo facilito Bootcamp on web development with .NET.

## Table of Contents

- [Overview](#overview)
   - [Summary](#summary)

- [Author](#author)

## Overview

### Summary

Develop an API using .NET, EF, and SQLite to manage the data handling of the .NET full-stack application called PairXpenses. The application is designed to be used by couples (partners, roommates, or spouses) to keep track of expenses that the couple is supposed to make monthly on items such as rent, utilities, groceries, among others. As the daily dynamics of the mentioned couples unfold, it is highly likely that one of the members ends up making more payments than the other during the month. This application allows establishing a monthly expense allocation protocol: from the classic 50/50 split to any other proportion of expenses that each couple feels comfortable establishing according to their monthly incomes and personal agreements. With a simple click, the application provides a report of monthly expenses as well as which member of the couple owes the other to settle the month and start the accounting for the next one. In addition to payments, the app also allows users to record debts that one user has with the other. Debts are a separate entity because they are not intended to be shared expenses but rather payments that one member made for the other and commits to repay at the end of the month.

This API has three main entities: User, Payment, and Debt. The API generates an SQLite database through migration called Database.db. The current database has some data by default, so to download the project and use it, I suggest to change user and password values. Likewise, the CORS configuration's WithOrigins in program.cs must be set up to receive the front-end app's localhost that will be using the API (You can use the PairXpenses front-end application created in Blazor in conjunction with this API as a reference). The API implements the necessary services and controllers to CRUD (Create, Read, Update, Delete) all the data in the database and provides methods to generate real-time reports of the monthly expenses for the couple using the app.

To run the API, you should use the command: `dotnet run` and wait for building. To see the Swagger version of the app, copy the URL localhost that is displayed on the bash, paste it in the browser, and add `/swagger` at the end.

### Project Requirements (as stated by Codigo facilito):

- Implement Auth using JWT.
- Data validation and mapping.
- Connect to a DB.
- Implement Serilog log creation.
- Utilize version control with Git and host the repository on GitHub.
- Write a README (using Markdown) that documents your project, including:
   - The project's purpose.
   - Technologies used.
   - Features.

#### Features

- Uses local Database.db with SQLite.
- Implements authentication and authorization and gives a JWT token to the front using the API.
- Allows 2 different couples to use the database with no interference of their data.
- The API is deployed on Azure to be used with the front-end app listed above.

### Links

- Solution URL: [GitHub Repository](https://github.com/light-roast/PairExpensesAPI)
- Azure URL: [Deployed on Azure](https://pairxpenses.azurewebsites.net/)

## Author

- Website: [Daniel Echeverri Llano](https://daniel-echeverri-portfolio.netlify.app/)
- Frontend Mentor: [@light-roast](https://www.frontendmentor.io/profile/light-roast)
