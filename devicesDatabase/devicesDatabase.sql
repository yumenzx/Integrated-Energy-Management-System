
CREATE DATABASE IF NOT EXISTS devicesDatabase;


USE devicesDatabase;


CREATE TABLE devicesData (
    id INT PRIMARY KEY AUTO_INCREMENT,
    description VARCHAR(255),
    address VARCHAR(255),
    maxHourlyConsumption INT,
    ownerId INT NULL
);

