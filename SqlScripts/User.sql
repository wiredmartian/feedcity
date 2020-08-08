CREATE TABLE Users (
	Id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
	FirstName VARCHAR(255) NOT NULL,
	LastName VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL UNIQUE,
	UserName VARCHAR(255) NOT NULL UNIQUE,
	LastSignIn TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP 
	HashedPassword VARCHAR(1000) NOT NULL,
	CreatedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
)