
CREATE DATABASE IF NOT EXISTS monitoringDatabase;


USE monitoringDatabase;


CREATE TABLE measurementData (
    id INT PRIMARY KEY AUTO_INCREMENT,
    timestampV DATETIME NOT NULL,
    device_id INT NOT NULL,
	measurement_value FLOAT NOT NULL
);

-- va trebui de afisat pe chart ora si consumul pt o data => trebuie stocata fiecare masurator apoi din backend
-- pt fiecare ora din acea data se va calcula consumul total de kw/h
-- la fiecare coada citia, se verifica doar ultimele 2 citiri pt verificarea dapsirii orare a consumului total
-- timestampul va fi de formatul an-luna-zi-ora . 