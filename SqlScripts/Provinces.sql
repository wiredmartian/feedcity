CREATE TABLE Provinces (
    Id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    CreatedOn TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
)

INSERT INTO Provinces VALUES ('Eastern Cape'),
('Western Cape'), ('Northern Cape'),
('North West'), ('KwaZulu-Natal'),
('Limpopo'), ('Free State'),
('Gauteng'), ('Mpumalanga')

