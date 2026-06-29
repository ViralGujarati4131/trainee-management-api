CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `LearningTasks` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExpectedTechStack` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DueDate` date NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `CreatedDate` datetime(6) NOT NULL,
        `UpdatedDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_LearningTasks` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `Mentors` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `LastName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Expertise` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `CreatedDate` datetime(6) NOT NULL,
        `UpdatedDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_Mentors` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `ProcessingJobs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `MessageId` char(36) COLLATE ascii_general_ci NOT NULL,
        `CorrelationId` char(36) COLLATE ascii_general_ci NOT NULL,
        `SubmissionId` int NOT NULL,
        `SubmissionFileId` int NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Attempts` int NOT NULL,
        `ErrorSummary` longtext CHARACTER SET utf8mb4 NULL,
        `RequestedAt` datetime(6) NOT NULL,
        `StartedAt` datetime(6) NULL,
        `CompletedAt` datetime(6) NULL,
        CONSTRAINT `PK_ProcessingJobs` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `Trainees` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FirstName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `LastName` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `TechStack` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `CreatedDate` datetime(6) NOT NULL,
        `UpdatedDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_Trainees` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Username` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Role` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `CreatedDate` datetime(6) NOT NULL,
        `UpdatedDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `TaskAssignments` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `TraineeId` int NOT NULL,
        `MentorId` int NOT NULL,
        `LearningTaskId` int NOT NULL,
        `AssignedDate` date NOT NULL,
        `DueDate` date NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Remarks` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_TaskAssignments` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_TaskAssignments_LearningTasks_LearningTaskId` FOREIGN KEY (`LearningTaskId`) REFERENCES `LearningTasks` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_TaskAssignments_Mentors_MentorId` FOREIGN KEY (`MentorId`) REFERENCES `Mentors` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_TaskAssignments_Trainees_TraineeId` FOREIGN KEY (`TraineeId`) REFERENCES `Trainees` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `Submissions` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `TaskAssignmentId` int NOT NULL,
        `SubmissionUrl` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Notes` longtext CHARACTER SET utf8mb4 NOT NULL,
        `SubmittedDate` date NOT NULL,
        `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Submissions` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Submissions_TaskAssignments_TaskAssignmentId` FOREIGN KEY (`TaskAssignmentId`) REFERENCES `TaskAssignments` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `Reviews` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `SubmissionId` int NOT NULL,
        `MentorId` int NOT NULL,
        `Feedback` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Score` int NOT NULL,
        `ReviewStatus` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `ReviewedDate` date NOT NULL,
        CONSTRAINT `PK_Reviews` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Reviews_Mentors_MentorId` FOREIGN KEY (`MentorId`) REFERENCES `Mentors` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Reviews_Submissions_SubmissionId` FOREIGN KEY (`SubmissionId`) REFERENCES `Submissions` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE TABLE `SubmissionFiles` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `SubmissionId` int NOT NULL,
        `OriginalFileName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `StorageFileName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ContentType` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Size` bigint NOT NULL,
        `Checksum` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UploadedByUserId` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreatedDate` datetime(6) NOT NULL,
        CONSTRAINT `PK_SubmissionFiles` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_SubmissionFiles_Submissions_SubmissionId` FOREIGN KEY (`SubmissionId`) REFERENCES `Submissions` (`Id`) ON DELETE RESTRICT
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_Reviews_MentorId` ON `Reviews` (`MentorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_Reviews_SubmissionId` ON `Reviews` (`SubmissionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_SubmissionFiles_SubmissionId` ON `SubmissionFiles` (`SubmissionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_Submissions_TaskAssignmentId` ON `Submissions` (`TaskAssignmentId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_TaskAssignments_LearningTaskId` ON `TaskAssignments` (`LearningTaskId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_TaskAssignments_MentorId` ON `TaskAssignments` (`MentorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE INDEX `IX_TaskAssignments_TraineeId` ON `TaskAssignments` (`TraineeId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    CREATE UNIQUE INDEX `IX_Users_Username` ON `Users` (`Username`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260625085121_ProcessingJobStatus') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260625085121_ProcessingJobStatus', '9.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

