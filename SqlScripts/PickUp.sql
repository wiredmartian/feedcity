CREATE TABLE PickupRequests (
    Id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    UserId INT(6) UNSIGNED,
    AcceptedBy INT(6) UNSIGNED,
    Location VARCHAR(255) NOT NULL,
    Latitude VARCHAR(50) NOT NULL,
    Longitude VARCHAR(50) NOT NULL,
    ContactName VARCHAR(255) NOT NULL,
    ContactNumber VARCHAR(50) NOT NULL,
    Status VARCHAR(50) NOT NULL,
    Notes VARCHAR(1000) NOT NULL,
    RequestedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    ClosedOn TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (AcceptedBy) REFERENCES Users(Id)
)