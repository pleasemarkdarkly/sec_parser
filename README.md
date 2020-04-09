### SEC Parser

This software detects individuals selling unregistered securities (laundering money) and the companies they use. This software was designed to leverage this insight to acquire short positions or hedge against the companies.  The activity this software identifies is also known as pumping and dumping.

To read the legal consequence [see SDNY Case 14-00399](https://cane.ashermadison.com/wp-content/uploads/sites/3/2014/07/20140715-us-v-cane-et-al-case-no-14-00399.pdf) and [another case](documents/Offshore_Accounts.pdf).

## Workflow

The user must provide one or more individuals or companies to seed the system. The application will download matching information from the SEC regulatory files. _This may take sigificant time and storage depending on the size._ The system downloads all regulatory filings from a company and any company matches of the individual.  Once the public filings have been processed. The user is presented with names and entities and charts of flagged activities.

This is an example.

![Diagram 1](documents/diagram001.png)

The left top diagram lists the regulatory activities over time. The left middle diagram maps individuals to the above activiites and others flagged by the system. The bottom left diagram is the public stock volume and price. These diagrams output a timeline around the pump and dump activities. The diagram on the right computes the total value of the scheme accomplished by the scheme.

The above scheme was further analyzed using actual insider stock ledgers and price breakdowns. [This](documents/Offshore_Accounts.pdf) document contains the stock ledger, the off-shore Cayman Island invoices, and an overview of the operations of the brokerage house [LOM Securities](https://www.lom.com/). The spreadsheet validates the software output.

![Diagram 1](documents/diagram002.png)

Additionally, the timelines track the _underlying_ company, or the entities prior to the instant shell company. The system also prioritizes companies with particular anomolies in the share volume after a number of reverse and forward splits, formations, and other questionable indicators. 

## Overview / Heuristics

A publicly traded company can create a subsidary with n shares. The subsidary can be spun out and left dormant. The bad actors then place the n stock certificates (aka bearer forms) into off-shore companies, discussed in detail above with [LOM Securities](https://www.lom.com/).

The dormant company shares are split across a number of _shell off-shore companies_, careful to not exceed __4%__ of the outstanding company stock.  [See SEC Rule 144](https://www.sec.gov/reportspubs/investor-publications/investorpubsrule144htm.html)  Obviously failure to disclose ownership, failure to register, and failure to report are all illegal.

During this phase the lawyer and company operator, the "bad actors," conduct a series of reverse splits on the stock. The bad actors then sell the shell to an unsuspecting legit company in what is called "reverse merger" transaction. During the capitalization of the new company, new investments are made.

The bad actors do not disclose the true volume of stock, the affiliated off-shore companies, or the fact that they control more than 50% of the outstanding stock. While the new company raise funds and conducts business, the share price appears to increase. Doing so, the new company believes they are adding value to their shareholders. 

In reality, the bad actors are making a series of "matched" trades to prop up the stock. When a larger volume of individuals participate in the purchase of stock, falsely believing the company is on the rise, the bad actors orchestrate the sale of their shares from the off-shore companies.  

Or the bad actors partner with a high-net worth indivdual to purchase large blocks of the off-shore shares to move his funds outside the United States.  In this scenario, the high network individual's entity making the investment can then later file bankruptcy and the wealthy individual can write off the "losses". In reality, this individual will just visit the Cayman Islands and meet the bad actors to pick up his money and pay the 10% surcharge.

## Pattern

By using entity matching within the SEC documents. This code identifies individuals, entities, activities, and share volume and price. A number of key activities are flagged such as reverse splits and forward splits. Additional markers are made and a model of the money laundering scheme is provided in the form of a timeline chart, elements in the chart are click-able to the underlying SEC filing.

## Application

This information can be used in a number of ways. 

1. Idenitfy individuals involved in financial schemes
2. Identify companies to short their stock
3. Hedgefund heuristics

## Disclaimer

This was written about 10 years ago, however, it appears that the scheme is still employed at some scale.





