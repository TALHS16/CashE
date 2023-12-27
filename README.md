# CashE
CashE was developed as a personal project to address the need for a more efficient and user-friendly solution in managing household finances. 
The primary goal was to streamline the expense tracking process, replacing the current messy and complicated Excel file method, especially for mobile use. 
Leveraging Unity for cross-platform development and incorporating Firebase for real-time data sharing among users. 
The project focused on identifying and addressing deficiencies in the existing tracking process.

## Log In
The login screen serves as the gateway to CashE. If you're not logged in or have previously logged out, you'll land on this screen as access to the app requires authentication. Once you log in, your information will be securely stored on the device, allowing for automatic login upon subsequent app access. The password is safeguarded using hash and salt functions to ensure the security of user credentials.

![IMG_9826](https://github.com/amxhab/CashE/assets/100534541/2c6f6fe7-ea49-4b58-9fc6-ebc6e80620d4)


## The Month
This is one of the 3 main screens, offering a visual representation of your expense and income distribution through a pie graph.

![IMG_9814](https://github.com/amxhab/CashE/assets/100534541/a5f9826f-b145-4f23-8492-fb7e387c4e38)

Clicking on a specific category reveals a detailed breakdown of transactions within that category.

![IMG_9816](https://github.com/amxhab/CashE/assets/100534541/8c333dea-61d8-469e-ba16-3b9b27e0bfe7)


 By pressing the (+) button at the bottom right, you can seamlessly add an expense or income.

![IMG_9815](https://github.com/amxhab/CashE/assets/100534541/bd352dae-819a-4529-bd4e-c4d7fccd2286)


The app supports real-time currency conversion through an external API.

![IMG_9828](https://github.com/amxhab/CashE/assets/100534541/a087ff1b-6c2e-40a0-a00a-e71818cdb9ce)


Specify the amount, currency, category, and description. If the description matches a previous transaction, the app intelligently assigns the category, though you have the flexibility to make changes.

![IMG_9829](https://github.com/amxhab/CashE/assets/100534541/0c4bb6ee-7533-4a23-85b9-91d31301e53c)

![IMG_9830](https://github.com/amxhab/CashE/assets/100534541/1a9447eb-3c64-46d6-b971-9e1c1df98b3f)

## Savings
The Savings screen, one of the three main sections in CashE, offers a robust set of features to enhance your financial planning.
The screen is divided into two parts.

![IMG_9818](https://github.com/amxhab/CashE/assets/100534541/0ae41b9e-720e-4c21-b53a-96b4c4eb0305)


The first part displays your savings between selected dates, with the ability to filter your savings data based on categories, individuals, and specific date ranges.

![IMG_9819](https://github.com/amxhab/CashE/assets/100534541/f84c4bce-de4f-44a1-9851-f30441e6302b)


The second part is dedicated to setting and managing financial targets. Easily add new targets by pressing the button, specifying whether they are weekly or monthly, the sum of the target and the category.

![IMG_9820](https://github.com/amxhab/CashE/assets/100534541/4796b148-4f01-411c-9ca5-ed40efd8fc2b)


Clicking on a target reveals its history, success rate, and allows for modifications or removal.

![WhatsApp Image 2023-12-27 at 14 08 04](https://github.com/amxhab/CashE/assets/100534541/a8055d3e-54f0-47ad-9272-bcba8a3061f5)


Monthly/Weekly Initialization: A Firebase cloud function ensures that your target sums are initialized on the first day of each month or week, depending on the target type. This seamless automation keeps the savings tracking up-to-date without manual intervention.

Target Completion Notifications: CashE cares about your financial goals. When a target surpasses 90%, a notification is sent to all users, ensuring everyone is aware of their imminent success. This proactive approach keeps the user engaged and motivated to achieve their financial targets.

## Statistics
Explore the robust Statistics screen in CashE, offering a multifaceted view of your financial habits and trends. 
The first part resembles a text conversation, presenting the most important information.

![IMG_9821](https://github.com/amxhab/CashE/assets/100534541/9e88ca48-841b-4837-8916-28d9f29b61f7)


The second part provides a detailed breakdown of averages, including category averages, monthly waste averages, and monthly profit averages.

![IMG_9822](https://github.com/amxhab/CashE/assets/100534541/29e7d7b6-8492-4062-b354-9802ebab9adb)


The third part offers insightful graphical representations of income, expenses, and profits for each month and for each user. The data points revealing the sum for each month.This part is for users could track changes over time.

![IMG_9824](https://github.com/amxhab/CashE/assets/100534541/83ccdc08-9257-4d8a-90b0-c0cbd03e6e70)









