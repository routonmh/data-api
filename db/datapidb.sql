CREATE DATABASE datapidb;

GRANT ALL PRIVILEGES ON datapidb.* TO 'serveruser'@'%' IDENTIFIED BY 'pass';

use datapidb;

CREATE TABLE user_account
(
    AccountID    CHAR(36)     NOT NULL PRIMARY KEY,
    Email        VARCHAR(128) NOT NULL,
    FirstName    VARCHAR(64)  NOT NULL,
    LastName     VARCHAR(64)  NOT NULL,
    PasswordHash VARCHAR(512) NOT NULL,
    DateCreated DATETIME     NOT NULL,
    IsActive     TINYINT      NOT NULL
);

CREATE TABLE user_session
(
    SessionID       CHAR(36) NOT NULL PRIMARY KEY,
    AccountID       CHAR(36) NOT NULL,
    DateIssued       DATETIME NOT NULL,
    DateLastRequest DATETIME NOT NULL,
    RequestCount    INT DEFAULT 0,
    CONSTRAINT FOREIGN KEY (AccountID)
        REFERENCES user_account (AccountID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE TABLE plant_entity
(
    PlantID        CHAR(36)     NOT NULL PRIMARY KEY,
    CommonName     VARCHAR(128) NOT NULL,
    ScientificName VARCHAR(128) NOT NULL,
    Description    VARCHAR(512)
);

CREATE TABLE plant_reported_location
(
    LocationID CHAR(36) NOT NULL,
    PlantID    CHAR(36) NOT NULL,
    Latitude   DECIMAL(10, 8),
    Longitude  DECIMAL(11, 8),
    INDEX (Latitude),
    INDEX (Longitude),
    CONSTRAINT FOREIGN KEY (PlantID)
        REFERENCES plant_entity (PlantID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

DELIMITER $$
CREATE TRIGGER onNewUserAccountCreated
    BEFORE INSERT
    ON user_account
    FOR EACH ROW
BEGIN
    IF (NEW.AccountID IS NULL) THEN
        SET NEW.AccountID = UUID();
    END IF;
    IF (NEW.DateCreated IS NULL) THEN
        SET NEW.DateCreated = NOW();
    END IF;
    IF (NEW.IsActive IS NULL) THEN
        SET NEW.IsActive = 1;
    END IF;
END$$

DELIMITER $$
CREATE TRIGGER onNewPlantEntityCreated
    BEFORE INSERT
    ON plant_entity
    FOR EACH ROW
BEGIN
    IF (NEW.PlantID IS NULL) THEN
        SET NEW.PlantID = UUID();
    END IF;
END$$

DELIMITER $$
CREATE TRIGGER onNewPlantReportedLocationCreated
    BEFORE INSERT
    ON plant_reported_location
    FOR EACH ROW
BEGIN
    IF (NEW.LocationID IS NULL) THEN
        SET NEW.LocationID = UUID();
    END IF;
END$$

# Sample Values
# Test user: test@test.com
# Test password: testing
INSERT INTO user_account
    (Email, FirstName, LastName, PasswordHash, DateCreated, IsActive)
    VALUES ('test@test.com', 'John', 'Doe',
            '1000:OqQt2NNgX9/b2CoPQDn35dUI1jlnOwQp:EomyYI9m9ergcDoVVKS3sfnUa9U=',
            NOW(), 1);

INSERT INTO plant_entity
    (CommonName, ScientificName, Description)
    VALUES ('Rose', 'Rosa', 'A red prickly flower.'),
           ('Palm Tree', 'Arecaceae', 'The famous Coconut Machine'),
           ('Butterfly Bush', 'Buddleja', 'A bush')