# Shopping-Website-MVC
Design and Develop a shopping website using ASP.NET MVC  and Mongodb. 
 
Before running the project on your pc, you need to have or install Mongodb on your system.
Please follow the steps to download and install Mongodb on your system: 
1. go to https://www.mongodb.com/download-center
2. MongoDB needs a folder to store the database. Create a C:\data\db\ directory: mkdir C:\data\db
3. Run "mongod.exe" in ~/MongoDB/Server/3.4/bin/mongod.exe 
4. Run "mongo.exe" in ~/MongoDB/Server/3.4/bin/mongo.exe

-------------------------------------------------------------------------------------------------
Note -> Default Mongo port number is 27017 (mongodb://127.0.0.1:27017). You can check the port number on mongo.exe
shell. If the port is different, you need to change it in the project, since project connects to 27017
string connectionString = "mongodb://localhost:27017";
for using Mongodb in my project, I installed a package: You don't need to install the package :)
Install-Package mongocsharpdriver
-------------------------------------------------------------------------------------------------
Please follow the steps to run the project on your pc: 

1. Run Shopping_Website.sln in Shopping_Website folder
2. Right click on the Solution, choose Build Solution
3. Run the project

-------------------------------------------------------------------------------------------------
Note: 
	Admin -> username: ssmith	Password: 12345
	Customer ->  username: ddao	Password: 12345
-------------------------------------------------------------------------------------------------
Optional -> Robo 3T (formerly Robomongo) is a shell-centric cross-platform MongoDB management tool
Link to download: https://robomongo.org/  
Please download Robo 3T
-------------------------------------------------------------------------------------------------
