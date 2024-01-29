
CREATE DATABASE IF NOT EXISTS usersDatabase;


USE usersDatabase;


CREATE TABLE usersData (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255),
    role VARCHAR(255),
    password VARCHAR(255)
);

INSERT INTO usersData (name, role, password) VALUES
    ('admin', 'administrator', 'qwert');