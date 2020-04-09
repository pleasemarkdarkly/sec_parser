### SEC Parser

This was used to catch an attorney launder money.

This software created timeline and support information which evidenced individuals involved in schemes to launder money through pumping and dumping of unregistered securities.  To read the legal consequence [see SEC Case and SDNY Case 14-00399](https://cane.ashermadison.com/wp-content/uploads/sites/3/2014/07/20140715-us-v-cane-et-al-case-no-14-00399.pdf) and see (documents/Offshore_Accounts.pdf).

## Workflow

The user must provide the target companies to the system. Then the system will download and process SEC regulatory files. _This takes both time and storage_ Once the public filings have been processed. The user is presented with names and entities and charts of flagged activities. This is an example.

![Diagram 1](documents/diagram001.png)

The left top diagram lists the regulatory activities over time. The left middle diagram maps individuals to the above activiites and others flagged by the system. The bottom left diagram is the public stock volume and price. These diagrams output a timeline around the pump and dump activities. The diagram on the right computes the total value of the scheme.

The above scheme was further analyzed using actual stock ledgers and price breakdowns. [This](documents/Offshore_Accounts.pdf) document contains the stock ledger, the off-shore Cayman Island invoices, and an overview of the operations of the brokerage house [LOM Securities](https://www.lom.com/). The spreadsheet validates the software output.

![Diagram 1](documents/diagram002.png)

Additionally, the timelines track the _underlying_ company, or the entities prior to the shell company. The system also prioritizes companies with anomolies in the share volume after a number of reverse and forward splits, formations, and other indicators. 

## Overview

A publicly traded company can create a subsidary with n shares. The subsidary can be spun out and left dormant. The bad actors then place the stock certificates into off-shore companies, discussed in detail above with [LOM Securities](https://www.lom.com/).

The dormant company shares are split across a number of _shell off-shore companies_, careful to not exceed __4%__ of the outstanding company stock.  Obviously failure to disclose ownership, failure to register, and failure to report are all illegal.

During this phase the lawyer and company operator, bad actors, conduct a series of reverse splits on the stock. The bad actors then sell the shell to a company in what is called "reverse merger" transaction. During the capitalization of the new company, new investments are made.

The bad actors do not disclose the true volume of stock, the off-shore companies, or the fact that they control more than 50% of the outstanding stock. While the new company raise funds and conducts business, the share price appears to increase. Doing so, the new company believes they are adding value to their shareholders. In reality, the bad actors are making a series of "matched" stock sales to prop the stock. When a larger volume of individuals participate in the purchase of stock, falsely believing the company is on the rise.  The bad actors orchestrate the sale of their shares from the off-shore companies.  

Or the bad actors  partner with a high-net worth indivdual to purchase large blocks of the off-shore shares to move his funds outside the United States.  In this scenario, the high network individual's entity making the investment can then later file bankruptcy and the wealthy individual can write off the "losses". In reality, this individual will just visit the Cayman Islands and meet the bad actors to pick up his money and pay the 10% surcharge.

## Pattern

By using entity matching within the SEC documents. This code matches individuals, entities, activities, and share volume and price. A number of key activities are flagged such as reverse splits and forward splits. Additional markers are made and a model of the money laundering is provided in the form of a timeline chart.

## Application

This information can be used in a number of ways. 

1. Idenitfy individuals involved in financial schemes
2. Identify companies to short their stock
3. Hedgefund heuristics

## MIT License





