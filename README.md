# **ExamenPropII** #

### Check the weather of any city in real time ###

![Main](https://user-images.githubusercontent.com/76973247/167517225-eeeabbf3-cc9b-4b1b-913a-094ae2e29909.png)

### Store your searched data in files ###

![DATA](https://user-images.githubusercontent.com/76973247/167517297-c84c949f-31d5-4bc0-850d-a4135f57413a.png)

### A statistic of how the day will be per hour (weather, wind, temp... etc) ###

![History](https://user-images.githubusercontent.com/76973247/167517170-9314c7ae-a31a-4e39-bdcc-071b8108d695.png)

## Technologies used: ## 

-Net Framework 4.8
-GunaChart 1.1 (required license) to check the statistics
-API https://openweathermap.org/ to check the weather
-AutoFac --> DependencyIngection

## App architecture ##

### ExamenPII2022.Domain ###

Contains entity classes POCO contracts and enums

![Domain](https://user-images.githubusercontent.com/76973247/167518341-af9dfd2a-cca2-4b62-a1e7-9a6110a10c5e.png)

Domain is very important as it connects AppCore to Infrastructure with dependency injection

### ExamenPII2022.Infraestructure ###

Contains the repositories to be used there the logic of the files is saved
got GetAll() and Add(T t) working

I used only those two methods of adding and getting a whole list because the program still works if we don't delete any records
If you want to implement this option to delete records you need to perform the method of T Get(T t) that returns only one object
The GetAll() is not very optimal because it brings with it the complete list of stored data (I did it like this because I was sleepy)

You can help yourself with the method I have in the RAFContext called Get(), it's private but you can figure it out!!

![GET](https://user-images.githubusercontent.com/76973247/167518903-a02c8893-9810-4a97-8e56-03d621edff06.png)

Remember that I only implement the add and getAll method because we do not delete records
You will also have to change some things in the forms that we will talk about later but let's move on to the AppCore

### ExamenPII2022.AppCore ###

There's not much to say here, this works magic hahaha... naaa

here we have the services and contracts folder
which basically this will help us connect to our view now let's move on to our Forms

###  ExamenPII2022.Fomrs ###

Well, there is a lot to improve here, starting with how we obtain the data:

![Charge](https://user-images.githubusercontent.com/76973247/167519457-5ac0f764-d962-44d2-849d-e3d25cdb0213.png)

Starting with that method that is obsolete, remembering to delete a record, you need to implement obtaining the object by ID in the best way
way so that you don't get null object exceptions what I mean by this the user has the opportunity to see their records from a DataGridView and this method
it would not work if a record is deleted so remember about Infrastructure there is the solution

There are also methods that must be done in the appcore as proccesses so identify them and do them haha ok the methods are:

UnixTimeStampToDateTime
CharTxtById necesita una actualización
ToUnixTime

## Cloning ##

git clone https://github.com/077708/ExamenPropII.git

